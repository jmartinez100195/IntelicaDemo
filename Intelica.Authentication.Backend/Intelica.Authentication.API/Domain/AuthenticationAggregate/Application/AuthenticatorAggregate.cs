using Intelica.Authentication.API.Common.DTO;
using Intelica.Authentication.API.Common.Encriptation;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
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
    public class AuthenticatorAggregate(IGenericCache genericCache, IGenericRSA genericRSA, IAuthenticationRepository repository,
    IOptionsSnapshot<JWTConfiguration> jwtConfiguration, IOptionsSnapshot<RSAConfiguration> rsaConfiguration) : IAuthenticatorAggregate
    {
        public AuthenticationResponse GenerateToken(BussinesUserResponse businessUserResponse, string clientID)
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
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthenticationResponse(jwt, "", true, "");
        }
        public BussinesUserResponse? ValidateCredentials(string businessUserEmail, string businessUserPassword, string publicKey)
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

        public ValidTokenResponse ValidToken(string token, string pageRoot, string httpVerb)
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
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var accessClaim = jwtSecurityToken.Claims.SingleOrDefault(x => x.Type.Equals(pageRoot));
                if (accessClaim == null) return new ValidTokenResponse(true, false, $"No tiene acceso al recurso {pageRoot} ");
                else
                {
                    var access = JsonSerializer.Deserialize<Access>(accessClaim.Value);
                    if (httpVerb.Equals("Post") && !access.CanCreate) { return new ValidTokenResponse(true, false, $"No tiene acceso a crear objetos en el recurso {pageRoot} "); }
                    if ((httpVerb.Equals("Put") || httpVerb.Equals("Patch")) && !access.CanUpdate) { return new ValidTokenResponse(true, false, $"No tiene acceso a actualizar objetos en el recurso {pageRoot} "); }
                    if (httpVerb.Equals("Delete") && !access.CanDelete) { return new ValidTokenResponse(true, false, $"No tiene acceso a eliminar objetos en el recurso {pageRoot} "); }
                }
            }
            catch (Exception ex)
            {
                return new ValidTokenResponse(true, false, ex.Message );
            }
            return new ValidTokenResponse(true, false, $"No tiene acceso al recurso {pageRoot} ");
        }
    }
}