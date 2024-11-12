using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito(int idProducto)
        {
            // Obtener el producto seleccionado
            var producto = await _context.productos.FindAsync(idProducto);
            if (producto == null)
            {
                return NotFound(); // Si no existe el producto
            }

            // Obtener el usuario logueado
            var usuario = await _context.usuarios.FirstOrDefaultAsync(u => u.nombre == User.Identity.Name);
            if (usuario == null)
            {
                return Unauthorized(); // Responder con estado de no autorizado si el usuario no está logueado
            }

            // Buscar si el producto ya está en el carrito
            var carritoExistente = await _context.carrito
                .FirstOrDefaultAsync(c => c.id_usuario == usuario.id_usuario && c.id_producto == idProducto);

            if (carritoExistente != null)
            {
                // Si ya existe, incrementar la cantidad y actualizar el precio total dinámicamente
                carritoExistente.cantidad += 1;
                carritoExistente.precio = carritoExistente.cantidad * producto.precio;

                _context.Update(carritoExistente);
            }
            else
            {
                // Si no existe, crear un nuevo elemento en el carrito con el precio total calculado
                var nuevoCarrito = new carrito
                {
                    id_usuario = usuario.id_usuario,
                    id_producto = idProducto,
                    cantidad = 1,
                    fecha = DateTime.UtcNow,
                    precio = producto.precio
                };

                _context.Add(nuevoCarrito);
            }

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(); // Responder con éxito
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
                    c.fecha
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
            var carrito = await _context.carrito.FindAsync(id);
            if (carrito != null)
            {
                _context.carrito.Remove(carrito);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool carritoExists(int id)
        {
            return _context.carrito.Any(e => e.carrito_id == id);
        }
    }
}
