using Intelica.Authentication.API.Common.Encriptation;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.DTO;
using Intelica.Authentication.API.Domain.AuthenticationAggregate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Intelica.Authentication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController(IGenericRSA customRSA, IAuthenticatorAggregate authenticator) : Controller
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
            var ip = Request.Host.Value;
            var response = authenticator.ValidateAuthentication(authenticationQuery, ip);
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
        public IActionResult RefresToken(RefreshTokenQuery refreshTokenQuery)
        {
            var ip = Request.Host.Value;
            var response = authenticator.ValidateRefreshToken(refreshTokenQuery.RefreshToken, refreshTokenQuery.BussinesUserEmail, refreshTokenQuery.ClientID, ip);
            return Ok(response);
        }
        [HttpGet]
        [Route("ValidateToken/{pageRoot}/{httpVerb}")]
        public IActionResult ValidateToken(string pageRoot, string httpVerb)
        {
            var token = Request.Headers.Authorization.ToString();
            var response = authenticator.ValidateToken(token, pageRoot, httpVerb);
            return Ok(response);
        }
    }
}