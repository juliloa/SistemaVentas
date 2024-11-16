using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;

namespace Sexshop_TutsiPop.Controllers
{
   
    public class carritoController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public carritoController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int carritoId, int nuevaCantidad)
        {
            using (var connection = (NpgsqlConnection)_context.Database.GetDbConnection())
            {
                connection.Open();

                // Verificar stock antes de actualizar
                using (var checkStockCommand = new NpgsqlCommand(@"
                    SELECT p.unidades_stock AS  Stock 
                    FROM carrito c 
                    JOIN productos p ON c.id_producto = p.id_producto 
                    WHERE c.carrito_id = @carrito_id", connection))
                {
                    checkStockCommand.Parameters.AddWithValue("carrito_id", carritoId);
                    var result = checkStockCommand.ExecuteScalar();

                    if (result == null)
                    {
                        return Json(new { success = false, message = "Producto no encontrado" });
                    }
                    int unidadesStock = (int)result;

                    if (nuevaCantidad > unidadesStock)
                    {
                        return Json(new { success = false, message = "No hay suficiente stock disponible" });
                    }
                }

                // Usar CommandType.Text para llamar a la función y obtener su retorno
                using (var command = new NpgsqlCommand("SELECT actualizar_cantidad_producto(@carrito_id, @nueva_cantidad)", connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("carrito_id", carritoId);
                    command.Parameters.AddWithValue("nueva_cantidad", nuevaCantidad);

                    // Ejecuta la función y obtiene el nuevo precio
                    var nuevoPrecio = (decimal)command.ExecuteScalar();

                    return Json(new { success = true, precio = nuevoPrecio });
                }
            }
        }




        [Authorize]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito(int idProducto)
        {
            // Obtener el producto seleccionado
            var producto = await _context.productos.FindAsync(idProducto);
            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToAction("IndexUsuario", "Home");
            }

            if (producto.unidades_stock <= 0)
            {
                TempData["Error"] = "El producto está agotado.";
                return RedirectToAction("IndexUsuario", "Home");
            }



            // Obtener el usuario logueado
            var usuario = await _context.usuarios.FirstOrDefaultAsync(u => u.nombre == User.Identity.Name);
            if (usuario == null)
            {
                return Unauthorized(); // Responder con estado de no autorizado si el usuario no está logueado
            }

            try
            {
                await _context.Database.ExecuteSqlRawAsync("CALL agregar_al_carrito({0}, {1})", idProducto, usuario.id_usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ya no hay más productos"; //ex.Message;
                return RedirectToAction("IndexUsuario", "Home");
            }

            // Redirigir al carrito
            return RedirectToAction("Index", "carrito");


        }
        // GET: carrito
        public async Task<IActionResult> Index()
        {
            var carrito = await _context.productosCarritoInfo
              .FromSqlRaw(@"SELECT 
                    c.carrito_id AS CarritoId, 
                    u.nombre AS UsuarioNombre, 
                    p.nombre_producto AS ProductoNombre, 
                    c.cantidad, 
                    c.precio AS Precio,
                    c.fecha,
                    p.unidades_stock AS Stock 
                FROM 
                    carrito c
                JOIN 
                    usuarios u ON c.id_usuario = u.id_usuario
                JOIN 
                    productos p ON c.id_producto = p.id_producto;

                ")
              .ToListAsync();

            return View("Index", carrito);

        }

        // GET: carrito/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.carrito
                .FirstOrDefaultAsync(m => m.carrito_id == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        // GET: carrito/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: carrito/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("carrito_id,id_usuario,id_producto,cantidad,fecha")] carrito carrito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carrito);
        }

        // GET: carrito/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.carrito.FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }
            return View(carrito);
        }

        // POST: carrito/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("carrito_id,id_usuario,id_producto,cantidad,fecha")] carrito carrito)
        {
            if (id != carrito.carrito_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!carritoExists(carrito.carrito_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carrito);
        }

        // GET: carrito/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.carrito
                .FirstOrDefaultAsync(m => m.carrito_id == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        // POST: carrito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Llamar al procedimiento almacenado para eliminar del carrito
                await _context.Database.ExecuteSqlRawAsync("CALL eliminar_del_carrito({0})", id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index)); // Redirigir al carrito

        }

        private bool carritoExists(int id)
        {
            return _context.carrito.Any(e => e.carrito_id == id);
        }
    }
}
