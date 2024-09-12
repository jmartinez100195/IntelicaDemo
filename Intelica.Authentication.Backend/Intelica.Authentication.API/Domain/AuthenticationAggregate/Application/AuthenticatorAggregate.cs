using Intelica.Authentication.API.Common.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Intelica.Authentication.API.Encriptation;
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
    IOptionsSnapshot<JWTConfiguration> jwtConfiguration) : IAuthenticatorAggregate
    {
        public AuthenticationResponse GenerateToken(BussinesUserResponse businessUserResponse)
        {

            List<Claim> claims = [];
            foreach (var businessUserPage in businessUserResponse.BusinessUserPages)
            {
                var accessLeve = JsonSerializer.Serialize(new { businessUserPage.CanCreate, businessUserPage.CanUpdate, businessUserPage.CanDelete });
                claims.Add(new(businessUserPage.PageRoot, accessLeve));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Value.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                                   claims: claims,
                                   expires: DateTime.UtcNow.AddDays(1),
                                   signingCredentials: cred
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthenticationResponse(jwt, "");
        }
        public BussinesUserResponse? ValidateCredentials(string businessUserEmail, string businessUserPassword, string publicKey)
        {
            var privateKey = genericCache.Get<string>(publicKey);
            if (privateKey == null) { return null; }
            string businessUserPasswordDecripted = genericRSA.Decript(privateKey, businessUserPassword);
            var businessUserPasswordEncripted = businessUserPassword;
            return repository.FindByCredentials(businessUserEmail, businessUserPasswordEncripted);
        }
    }
}