using Intelica.Authentication.API.Common.DTO;
using Intelica.Authentication.API.Common.Encriptation;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Intelica.Infrastructure.Library.Cache.Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
namespace Intelica.Authentication.API.Domain.AuthenticationAggregate.Application
{
    public class AuthenticatorAggregate(IGenericCache genericCache, IGenericRSA genericRSA, IAuthenticationRepository repository,
    IOptionsSnapshot<JWTConfiguration> jwtConfiguration, IOptionsSnapshot<RSAConfiguration> rsaConfiguration) : IAuthenticatorAggregate
    {
        public AuthenticationResponse GenerateToken(BussinesUserResponse businessUserResponse)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Value.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = [];
            foreach (var businessUserPage in businessUserResponse.BusinessUserPages)
            {
                var accessLeve = JsonSerializer.Serialize(new { businessUserPage.CanCreate, businessUserPage.CanUpdate, businessUserPage.CanDelete });
                claims.Add(new(businessUserPage.PageRoot, accessLeve));
            }
            var token = new JwtSecurityToken(
                issuer: "auth.intelica.com",
                audience: "intelica.com",
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthenticationResponse(jwt, "");
        }
        public BussinesUserResponse? ValidateCredentials(string businessUserEmail, string businessUserPassword, string publicKey)
        {
            var privateKey = genericCache.Get<string>(publicKey);
            if (privateKey == null) { return null; }
            string businessUserPasswordDecripted = genericRSA.Decript(privateKey, businessUserPassword);
            string businessUserPasswordEncripted = genericRSA.Encript(rsaConfiguration.Value.PublicKey, businessUserPasswordDecripted);
            return repository.FindByCredentials(businessUserEmail, businessUserPasswordEncripted);
        }
    }
}