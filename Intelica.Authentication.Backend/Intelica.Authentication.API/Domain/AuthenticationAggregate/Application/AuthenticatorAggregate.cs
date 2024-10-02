using Intelica.Authentication.API.Common.DTO;
using Intelica.Authentication.API.Common.Encriptation;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Domain;
using Intelica.Authentication.API.Domain.ClientAggregate.Application.Interfaces;
using Intelica.Infrastructure.Library.Cache.Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application
{
    public class AuthenticatorAggregate(IGenericCache genericCache, IGenericRSA genericRSA, IClientAggregate client, IAuthenticationRepository repository,
    IOptionsSnapshot<JWTConfiguration> jwtConfiguration, IOptionsSnapshot<RSAConfiguration> rsaConfiguration) : IAuthenticatorAggregate
    {
        public AuthenticationResponse ValidateAuthentication(AuthenticationQuery authenticationQuery, string ip)
        {
            if (!client.IsValid(authenticationQuery.ClientID, authenticationQuery.CallBack)) return new AuthenticationResponse("", "", false, false);
            var businessUser = ValidateCredentials(authenticationQuery.BusinessUserEmail, authenticationQuery.BusinessUserPassword, authenticationQuery.PublicKey);
            if (businessUser == null) return new AuthenticationResponse("", "", false, false);
            var token = GenerateToken(businessUser, ip, authenticationQuery.ClientID);
            var refreshToken = GenerateRefreshToken(businessUser.BusinessUserID, ip);
            return new AuthenticationResponse(token, refreshToken, true, businessUser.BusinessUserFirstLogin); ;
        }
        public RefreshTokenResponse ValidateRefreshToken(Guid refreshToken, string businessUserEmail, string clientID, string ip)
        {
            var accesInformation = repository.FindAccessInformation(refreshToken);
            if (accesInformation == null) return new RefreshTokenResponse(false, false, "");
            if (!accesInformation.IP.Equals(ip)) return new RefreshTokenResponse(false, false, "");
            if (DateTime.Now > accesInformation.ExpirationDate) return new RefreshTokenResponse(true, true, "");
            var businessUser = repository.FindByEmail(businessUserEmail);
            var token = GenerateToken(businessUser, ip, clientID);
            return new RefreshTokenResponse(true, true, token);
        }
        public ValidTokenResponse ValidateToken(string token, string pageRoot, string httpVerb)
        {
            if (token.Contains("Bearer")) token = token.Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfiguration.Value.Key);
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var accessClaim = jwtSecurityToken.Claims.SingleOrDefault(x => x.Type.Equals(pageRoot));
                if (accessClaim == null) return new ValidTokenResponse(true, false);
                else
                {
                    var access = JsonSerializer.Deserialize<Access>(accessClaim.Value);
                    if (httpVerb.Equals("POST") && !access.CanCreate) { return new ValidTokenResponse(true, false); }
                    if ((httpVerb.Equals("PUT") || httpVerb.Equals("PATCH")) && !access.CanUpdate) { return new ValidTokenResponse(true, false); }
                    if (httpVerb.Equals("DELETE") && !access.CanDelete) { return new ValidTokenResponse(true, false); }
                }
            }
            catch
            {
                return new ValidTokenResponse(false, false);
            }
            return new ValidTokenResponse(true, true);
        }
        #region Private
        private string GenerateToken(BussinesUserResponse businessUserResponse, string ip, string clientID)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Value.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = [];
            foreach (var businessUserPage in businessUserResponse.BusinessUserPages)
            {
                var accessLevel = JsonSerializer.Serialize(new Access(businessUserPage.CanCreate, businessUserPage.CanUpdate, businessUserPage.CanDelete));
                claims.Add(new(businessUserPage.PageRoot, accessLevel));
            }
            claims.Add(new("preferred_username", businessUserResponse.BusinessUserName));
            claims.Add(new("sub", businessUserResponse.BusinessUserID.ToString()));
            var token = new JwtSecurityToken(
                issuer: "auth.intelica.com",
                audience: clientID,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private string GenerateRefreshToken(Guid businessUserID, string ip)
        {
            var accesInformation = new AccessInformation(businessUserID, ip, DateTime.Now.AddMinutes(720));
            repository.CreateAccessInformation(accesInformation);
            repository.SaveChanges();
            return accesInformation.AccessInformationID.ToString();
        }
        private BussinesUserResponse? ValidateCredentials(string businessUserEmail, string businessUserPassword, string publicKey)
        {
            var privateKey = genericCache.Get<string>(publicKey);
            if (privateKey == null) { return null; }
            string businessUserPasswordDecripted = genericRSA.Decript(privateKey, businessUserPassword);
            var bussinesUser = repository.FindByEmail(businessUserEmail);
            if (bussinesUser != null)
            {
                string businessUserResponsePasswordDecripted = genericRSA.Decript(rsaConfiguration.Value.PrivateKey, bussinesUser.BusinessUserPassword);
                if (businessUserPasswordDecripted == businessUserResponsePasswordDecripted) return bussinesUser;
            }
            return null;
        }
        #endregion        
    }
}