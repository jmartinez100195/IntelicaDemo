using Microsoft.AspNetCore.Mvc;

namespace Intelica.Authentication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : Controller
    {
        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }
    }
}