using Intelica.Authentication.API.Common.DTO;
using Intelica.Authentication.API.Common.Encriptation;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace Intelica.Authentication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController(IGenericRSA customRSA, IAuthenticatorAggregate authenticator, IOptionsSnapshot<RSAConfiguration> rsaConfiguration) : Controller
    {
        [HttpGet]
        public IActionResult GetPublicKey()
        {
            var keyValue = customRSA.GetPublicKey();
            return Ok(new { publicKey = keyValue.Key });
        }
        [HttpPost]
        public IActionResult ValidateAuthentication(AuthenticationQuery authenticationQuery)
        {
            var businessUserID = authenticator.ValidateCredentials(authenticationQuery.BusinessUserEmail, authenticationQuery.BusinessUserPassword, authenticationQuery.PublicKey);
            if (businessUserID == null) return Unauthorized();
            var response = authenticator.GenerateToken(businessUserID);
            return Ok(response);
        }
        [HttpPost]
        public IActionResult RefresToken() { 

        }
        [HttpGet]
        [Route("{value}")]
        public IActionResult EdDecrypt(string value)
        {
            var encryptedValue = customRSA.Encript(rsaConfiguration.Value.PublicKey, value);
            var decryptedValue = customRSA.Decript(rsaConfiguration.Value.PrivateKey, encryptedValue);
            //var key = customRSA.GetPublicKey();
            //var encryptedValue = customRSA.Encript(key.Key, value);
            //var decryptedValue = customRSA.Decript(key.Value, encryptedValue);
            return Ok(value == decryptedValue);
        }
    }
}