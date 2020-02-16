using Microsoft.AspNetCore.Mvc;

namespace AspDotNetCoreDemo.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}