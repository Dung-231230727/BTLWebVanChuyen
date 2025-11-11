using Microsoft.AspNetCore.Mvc;

namespace WebVanChuyen.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
