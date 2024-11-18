using System;
using System.Collections.Generic;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
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
        [Authorize]
        public async Task<IActionResult> RealizarCompra(clientes cliente, int? empleadoId)
        {
            var usuario = await _context.usuarios.FirstOrDefaultAsync(u => u.nombre == User.Identity.Name);
            if (usuario == null)
            {
                return Unauthorized();
            }
            var clienteExistente = await _context.clientes
        .FirstOrDefaultAsync(c => c.correo == usuario.email);

            var carrito = await _context.carrito
                .Where(c => c.id_usuario == usuario.id_usuario)
                .ToListAsync();

            if (!carrito.Any())
            {
                return RedirectToAction("IndexUsuario", "Home");
            }

            var empleado = empleadoId.HasValue
            ? await _context.empleados.FirstOrDefaultAsync(e => e.cedula_empleado == empleadoId.Value.ToString()) // Convertir a string
            : await _context.empleados.OrderBy(e => Guid.NewGuid()).FirstOrDefaultAsync();

            await _context.SaveChangesAsync();

            // Crear la venta
            var venta = new ventas
            {
                cedula_cliente = clienteExistente.cedula_cliente,
                cedula_empleado = empleado.cedula_empleado,
                fecha_venta = DateTime.UtcNow
            };

            _context.ventas.Add(venta);
            await _context.SaveChangesAsync();
            int idVenta = venta.id_venta;
            if (venta.id_venta == 0)
            {
                // Esto indica que no se ha asignado correctamente el ID, lo cual es muy raro
                return BadRequest("Hubo un problema al guardar la venta.");
            }
            // Crear los detalles de la venta
            foreach (var item in carrito)
            {
                var detalle = new detalle_venta
                {
                    id_venta = venta.id_venta,
                    id_producto = item.id_producto,
                    cantidad = item.cantidad,
               
                };
                _context.detalle_venta.Add(detalle);
            }

            await _context.SaveChangesAsync();

            // Limpiar el carrito
            _context.carrito.RemoveRange(carrito);
            await _context.SaveChangesAsync();

            // Generar la factura en PDF
            return await GenerarFacturaPDF(idVenta);
        }

        public async Task<IActionResult> GenerarFacturaPDF(int idVenta)
        {
            // Obtener la venta con detalles de productos, cliente, y empleado
            var venta = await (from v in _context.ventas
                               where v.id_venta == idVenta
                               join d in _context.detalle_venta on v.id_venta equals d.id_venta
                               join p in _context.productos on d.id_producto equals p.id_producto
                               join c in _context.clientes on v.cedula_cliente equals c.cedula_cliente
                               join e in _context.empleados on v.cedula_empleado equals e.cedula_empleado
                               join mp in _context.metodos_pago on v.id_metodo_pago equals mp.id_metodo into mpJoin
                               from mp in mpJoin.DefaultIfEmpty() // Esto asegura que incluso si el metodo de pago es null, se incluya
                               where v.id_venta == idVenta // Asegúrate de que el ID de la venta sea correcto
                               select new
                               {
                                   v.id_venta,
                                   v.fecha_venta,
                                   ClienteNombre = c.nombre,
                                   ClienteApellido = c.apellido,
                                   ClienteCorreo = c.correo,
                                   ClienteDireccion = c.id_direccion,
                                   EmpleadoNombre = e.nombre,
                                   EmpleadoApellido = e.apellido,
                                   MetodoPago = mp != null ? mp.metodo_pago : "No especificado", // Maneja el caso donde mp es null
                                   Detalles = new
                                   {
                                       ProductoNombre = p.nombre_producto,
                                       d.cantidad,
                                       p.precio,
                                       Subtotal = (d.cantidad * p.precio * (1 - d.descuento))
                                   }
                               }).ToListAsync();


            if (venta == null || !venta.Any())
            {
                return NotFound();
            }

            using (var stream = new MemoryStream())
            {
                var documento = new Document();
                PdfWriter.GetInstance(documento, stream);
                documento.Open();

                // Encabezado de la factura
                documento.Add(new Paragraph("Factura de Compra"));
                documento.Add(new Paragraph($"ID Venta: {venta.First().id_venta}"));
                documento.Add(new Paragraph($"Fecha: {venta.First().fecha_venta.ToString("dd/MM/yyyy")}"));
                documento.Add(new Paragraph($"Empleado: {venta.First().EmpleadoNombre} {venta.First().EmpleadoApellido}"));
                documento.Add(new Paragraph($"Método de Pago: {venta.First().MetodoPago}"));
                documento.Add(new Paragraph("\n"));

                // Información del Cliente
                var cliente = venta.First();
                documento.Add(new Paragraph($"Cliente: {cliente.ClienteNombre} {cliente.ClienteApellido}"));
                documento.Add(new Paragraph($"Correo: {cliente.ClienteCorreo}"));
                documento.Add(new Paragraph($"Dirección: {(cliente.ClienteDireccion.HasValue ? cliente.ClienteDireccion.ToString() : "Sin Dirección")}"));
                documento.Add(new Paragraph("\n"));

                // Tabla con detalles de la venta
                var tabla = new PdfPTable(4);
                tabla.AddCell("Producto");
                tabla.AddCell("Cantidad");
                tabla.AddCell("Precio Unitario");
                tabla.AddCell("Subtotal");

                // Añadir detalles de los productos a la tabla
                decimal totalVenta = 0;
                foreach (var detalle in venta)
                {
                    tabla.AddCell(detalle.Detalles.ProductoNombre);
                    tabla.AddCell(detalle.Detalles.cantidad.ToString());
                    tabla.AddCell(detalle.Detalles.precio.ToString("C"));
                    tabla.AddCell(detalle.Detalles.Subtotal.ToString("C"));
                    totalVenta += detalle.Detalles.Subtotal;
                }

                documento.Add(tabla);
                documento.Add(new Paragraph("\n"));

                // Total de la venta
                documento.Add(new Paragraph($"Total: {totalVenta.ToString("C")}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));

                // Cerrar el documento PDF
                documento.Close();

                // Devolver el PDF como archivo de descarga
                return File(stream.ToArray(), "application/pdf", "Factura_" + venta.First().id_venta + ".pdf");
            }
        }
        public async Task<IActionResult> FinalizarCompra()
        {
            // Obtén el usuario actual
            var usuario = await _context.usuarios
                .FirstOrDefaultAsync(u => u.nombre == User.Identity.Name);

            if (usuario == null)
            {
                return Unauthorized(); // Si no existe el usuario, redirige a la página de error o login
            }

            // Busca al cliente usando el correo del usuario
            var cliente = await _context.clientes
                .FirstOrDefaultAsync(c => c.correo == usuario.email);  // Comparar por correo

            if (cliente == null)
            {
                // Si no se encuentra el cliente, redirige a la vista para crear un cliente
                return RedirectToAction("CrearCliente");
            }

            // Obtén los productos del carrito del usuario
            var carrito = await _context.carrito
                .Where(c => c.id_usuario == usuario.id_usuario)  // Asegúrate de que el id_usuario del carrito coincida
                .ToListAsync();

            // Calcula el total de la compra
            var totalCompra = carrito.Sum(item => item.precio * item.cantidad);

            // Crea el ViewModel para pasar a la vista
            var viewModel = new FinalizarCompraViewModel
            {
                Carrito = carrito,
                Cliente = cliente,
                TotalCompra = totalCompra
            };

            // Devuelve la vista con los datos del carrito, cliente y total
            return View(viewModel);
        }

        public IActionResult CrearCliente()
        {
            // Inicializamos el modelo para asegurarnos de que no sea null
            var modelo = new clientes(); // Crea una nueva instancia del modelo
            return View(modelo); // Pasa el modelo a la vista
        }


        [HttpPost]
        public async Task<IActionResult> GuardarCliente(clientes cliente)
        {
            // Verificar si el cliente ya existe usando la cédula o algún otro identificador
            var clienteExistente = await _context.clientes
                                   .FirstOrDefaultAsync(c => c.cedula_cliente == cliente.cedula_cliente);

            if (clienteExistente != null)
                {
                    // Si el cliente ya existe, redirigir directamente a 'FinalizarCompra'
                    return RedirectToAction("FinalizarCompra", new { clienteId = cliente.cedula_cliente });
                }

            if (ModelState.IsValid)
            {
                // Si el cliente no existe, agregarlo a la base de datos
                _context.clientes.Add(cliente);
                await _context.SaveChangesAsync();

                // Redirigir a la vista de 'FinalizarCompra' con el cliente recién creado
                return RedirectToAction("FinalizarCompra", new { clienteId = cliente.cedula_cliente });
            }

            // Si hay un error en el modelo, volver a la vista de creación
            return View(cliente);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GuardarDireccion(direcciones direccion)
        {
            var usuario = await _context.usuarios.FirstOrDefaultAsync(u => u.nombre == User.Identity.Name);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no autorizado" });
            }

            direccion.id_direccion = 0; // Aseguramos que se cree un nuevo registro
            _context.direcciones.Add(direccion);
            await _context.SaveChangesAsync();

            // Actualizar el cliente con la nueva dirección
            var cliente = await _context.clientes.FirstOrDefaultAsync(c => c.cedula_cliente == usuario.nombre);
            if (cliente != null)
            {
                // Si el cliente existe, asociamos la nueva dirección
                cliente.id_direccion = direccion.id_direccion;
                _context.Update(cliente);
                await _context.SaveChangesAsync();

                // Redirigir a la vista de 'Finalizarcompra' con el cliente ya existente
                return RedirectToAction("Finalizarcompra", new { clienteId = cliente.cedula_cliente });
            }
            else
            {
                // Si el cliente no existe, redirigir a la vista para crear el cliente
                return RedirectToAction("CrearCliente");
            }
        }


        [HttpPost]
        public IActionResult UpdateQuantity(int carrito_id, int nuevaCantidad)
        {
            using (var connection = (NpgsqlConnection)_context.Database.GetDbConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Usar función almacenada para actualizar y devolver nuevo precio
                        using (var command = new NpgsqlCommand(
                            "SELECT actualizar_cantidad_producto(@carrito_id, @nueva_cantidad)", connection, transaction))
                        {
                            command.Parameters.AddWithValue("carrito_id", carrito_id);
                            command.Parameters.AddWithValue("nueva_cantidad", nuevaCantidad);

                            var nuevoPrecio = (decimal)command.ExecuteScalar();

                            // Confirmar transacción
                            transaction.Commit();

                            return Json(new { success = true, precio = nuevoPrecio });
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(new { success = false, message = "Error al actualizar la cantidad", error = ex.Message });
                    }
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
