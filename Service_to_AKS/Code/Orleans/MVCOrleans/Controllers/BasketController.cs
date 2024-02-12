using Microsoft.AspNetCore.Mvc;

namespace MVCOrleans.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
