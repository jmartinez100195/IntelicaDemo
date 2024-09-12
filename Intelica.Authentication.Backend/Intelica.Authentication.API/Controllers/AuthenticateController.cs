using Intelica.Authentication.API.Common.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Intelica.Authentication.API.Encriptation;
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
            var publicKey = customRSA.GetPublicKey();
            return Ok(new { publicKey });
        }
        [HttpPost]
        public IActionResult ValidateAuthentication(AuthenticationQuery authenticationQuery)
        {
            var businessUserID = authenticator.ValidateCredentials(authenticationQuery.BusinessUserEmail, authenticationQuery.BusinessUserPassword, authenticationQuery.PublicKey);
            if (businessUserID == null) return Unauthorized();
            var response = authenticator.GenerateToken(businessUserID);
            return Ok(response);
        }
        [HttpGet]
        [Route("{value}")]
        public IActionResult EdDecrypt(string value)
        {
            var key = customRSA.GetPublicKey();
            var encrypt = customRSA.Encript(key, value);
            var decrypt = customRSA.Decript(key, value);
            return Ok(encrypt);
        }
    }
}