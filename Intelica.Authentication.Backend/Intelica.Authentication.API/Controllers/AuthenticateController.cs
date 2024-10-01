using Intelica.Authentication.API.Common.Encriptation;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Intelica.Authentication.API.Domain.ClientAggregate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Intelica.Authentication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController(IGenericRSA customRSA, IAuthenticatorAggregate authenticator, IClientAggregate client) : Controller
    {
        [HttpGet]
        public IActionResult GetPublicKey()
        {
            var keyValue = customRSA.GetKeys();
            return Ok(new { publicKey = keyValue.Key });
        }
        [HttpPost]
        public IActionResult ValidateAuthentication(AuthenticationQuery authenticationQuery)
        {
            if (!client.IsValid(authenticationQuery.ClientID, authenticationQuery.CallBack)) return Ok(new AuthenticationResponse("", "", false, "No se pudo validar el origen de la metadata", false));
            var businessUser = authenticator.ValidateCredentials(authenticationQuery.BusinessUserEmail, authenticationQuery.BusinessUserPassword, authenticationQuery.PublicKey);
            if (businessUser == null) return Ok(new AuthenticationResponse("", "", false, "Usuario o contrasena invalida", false));
            var response = authenticator.GenerateToken(businessUser, authenticationQuery.ClientID);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Expires = DateTime.Now.AddDays(1),
                Secure = true, // Es necesario para SameSite=None
                SameSite = SameSiteMode.None, // Permitir envío entre diferentes sitios
            };
            Response.Cookies.Append("token", response.JwtToken, cookieOptions);
            Response.Cookies.Append("refresToken", response.RefreshToken, cookieOptions);
            return Ok(response);
        }
        [HttpPost]
        [Route("RefresToken")]
        public IActionResult RefresToken()
        {
            return Ok();
        }
        [HttpGet]
        [Route("ValidToken/{pageRoot}/{httpVerb}")]
        public IActionResult ValidToken(string pageRoot, string httpVerb)
        {
            var token = Request.Headers.Authorization.ToString();
            var response = authenticator.ValidToken(token, pageRoot, httpVerb);
            return Ok(response);
        }
    }
}