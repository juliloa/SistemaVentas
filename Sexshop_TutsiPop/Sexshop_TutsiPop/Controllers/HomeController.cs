using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sexshop_TutsiPop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Sexshop_TutsiPopContext _context;

        public HomeController(Sexshop_TutsiPopContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // Acción pública: se muestra como página de inicio si no está autenticado
        public IActionResult Alerta()
        {
            return View();
        }

        // Acción de administrador, accesible solo para usuarios autenticados
        [Authorize(Roles = "administrador")]
        public IActionResult Admin()
        {
            return View();
        }

        // Acción protegida, accesible solo para usuarios autenticados
        [Authorize (Roles = "cliente")]
        public async Task<IActionResult> IndexUsuario()
        {
            var productosinfo = await _context.productosInfo
                .FromSqlRaw(@"SELECT 
            p.id_producto,
            p.nombre_producto,
            c.nombre_categoria AS categoria,
            pr.nombre_empresa AS proveedor,
            p.unidades_stock,
            p.precio,
            p.activo,
            p.imagen_url
        FROM 
            productos p
        JOIN 
            categorias c ON p.id_categoria = c.id_categoria
        JOIN 
            proveedores pr ON p.id_proveedor = pr.id_proveedor;")
                .ToListAsync();

            // Check if productosinfo is null
            if (productosinfo == null)
            {
                productosinfo = new List<productosInfo>(); // Initialize an empty list to prevent null reference
                Console.WriteLine("No se encontraron productos.");
            }

            return View(productosinfo);
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
