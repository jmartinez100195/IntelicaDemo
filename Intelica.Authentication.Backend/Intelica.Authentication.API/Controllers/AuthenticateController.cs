using Microsoft.AspNetCore.Mvc;

namespace Intelica.Authentication.API.Controllers
{
    public class AuthenticateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
