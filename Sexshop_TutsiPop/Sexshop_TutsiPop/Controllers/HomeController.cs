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

        // Acci�n p�blica: se muestra como p�gina de inicio si no est� autenticado
        public IActionResult Alerta()
        {
            return View();
        }

        // Acci�n de administrador, accesible solo para usuarios autenticados
        [Authorize]
        public IActionResult Admin()
        {
            return View();
        }

        // Acci�n protegida, accesible solo para usuarios autenticados
        [Authorize]
        public IActionResult IndexUsuario()
        {
            return View();
        }

        // P�gina de inicio: accesible solo si est� autenticado
       
        public IActionResult Index()
        {
            return View();
        }

        // P�gina de privacidad: puede ser p�blica o protegida si se desea
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
