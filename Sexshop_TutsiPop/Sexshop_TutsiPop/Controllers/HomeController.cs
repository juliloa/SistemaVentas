using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sexshop_TutsiPop.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sexshop_TutsiPop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Acción pública: se muestra como página de inicio si no está autenticado
        public IActionResult Alerta()
        {
            return View();
        }

        // Acción de administrador, accesible solo para usuarios autenticados
        [Authorize]
        public IActionResult Admin()
        {
            return View();
        }

        // Acción protegida, accesible solo para usuarios autenticados
        [Authorize]
        public IActionResult IndexUsuario()
        {
            return View();
        }

        // Página de inicio: accesible solo si está autenticado
       
        public IActionResult Index()
        {
            return View();
        }

        // Página de privacidad: puede ser pública o protegida si se desea
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
