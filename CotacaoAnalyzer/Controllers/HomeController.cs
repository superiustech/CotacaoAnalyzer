using Microsoft.AspNetCore.Mvc;

namespace CotacaoAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
