using Microsoft.AspNetCore.Mvc;

namespace App_client.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
