using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;
using Microsoft.AspNetCore.Authorization;

namespace Sexshop_TutsiPop.Controllers
{
    [Authorize(Roles = "administrador")]
    public class pedidosController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public pedidosController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }
        //// Acción para exportar datos a Excel
        //public IActionResult ExportToExcel()
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    var Empleados = _context.empleados.ToList();

        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("pedidos");
        //        worksheet.Cells[1, 1].Value = "Cedula Empleado";
        //        worksheet.Cells[1, 2].Value = "Nombre";
        //        worksheet.Cells[1, 3].Value = "Apellido";
        //        worksheet.Cells[1, 4].Value = "Correo";
        //        worksheet.Cells[1, 5].Value = "Fecha Contratacion";
        //        worksheet.Cells[1, 6].Value = "Rol";
        //        worksheet.Cells[1, 7].Value = "Salario";

        //        using (var range = worksheet.Cells[1, 1, 1, 7])
        //        {
        //            range.Style.Font.Bold = true;
        //            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightPink);
        //            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }

        //        int row = 2;
        //        foreach (var empleados in Empleados)
        //        {
        //            worksheet.Cells[row, 1].Value = empleados.cedula_empleado;
        //            worksheet.Cells[row, 2].Value = empleados.nombre;
        //            worksheet.Cells[row, 3].Value = empleados.apellido;
        //            worksheet.Cells[row, 4].Value = empleados.correo;
        //            worksheet.Cells[row, 5].Value = empleados.fecha_contratacion.ToString("o"); // Formato UTC
        //            worksheet.Cells[row, 6].Value = empleados.rol;
        //            worksheet.Cells[row, 7].Value = empleados.salario;
        //            row++;
        //        }

        //        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        //        var stream = new MemoryStream();
        //        package.SaveAs(stream);
        //        stream.Position = 0;
        //        string excelName = $"pedidos-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}.xlsx";

        //        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        //    }
        //}
        // GET: pedidos
        public async Task<IActionResult> Index()
        {
            var pedidosinfo = await _context.pedidoinfos
                .FromSqlRaw(@"SELECT p.id_pedido AS IdPedido,
                         pr.nombre_empresa AS NombreProveedor,
                         ep.nombre_estado AS EstadoPedido,
                         mp.metodo_pago AS MetodoPago,
                         d.direccion_calle AS DireccionEntrega,
                         d.barrio AS Barrio,
                         d.ciudad AS Ciudad,
                         d.codigo_postal AS CodigoPostal,
                         d.pais AS Pais,
                         p.fecha_pedido AS FechaPedido
                  FROM pedidos p
                  JOIN proveedores pr ON p.id_proveedor = pr.id_proveedor
                  JOIN estado_pedido ep ON p.id_estado = ep.id_estado
                  LEFT JOIN metodos_pago mp ON p.id_metodo_pago = mp.id_metodo
                  JOIN direcciones d ON p.id_direccion = d.id_direccion;")
                .ToListAsync();

            return View("Index", pedidosinfo);
            //return View(await _context.pedidos.ToListAsync());
        }

        // GET: pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.pedidoinfos.
                FromSqlRaw(@"SELECT p.id_pedido AS ""IdPedido"",
                         pr.nombre_empresa AS ""NombreProveedor"",
                         ep.nombre_estado AS ""EstadoPedido"",
                         mp.metodo_pago AS ""MetodoPago"",
                         d.direccion_calle AS ""DireccionEntrega"",
                         d.barrio AS ""Barrio"",
                         d.ciudad AS ""Ciudad"",
                         d.codigo_postal AS ""CodigoPostal"",
                         d.pais AS ""Pais"",
                         p.fecha_pedido AS ""FechaPedido""
                  FROM pedidos p
                  JOIN proveedores pr ON p.id_proveedor = pr.id_proveedor
                  JOIN estado_pedido ep ON p.id_estado = ep.id_estado
                  LEFT JOIN metodos_pago mp ON p.id_metodo_pago = mp.id_metodo
                  JOIN direcciones d ON p.id_direccion = d.id_direccion
                  WHERE p.id_pedido = {0}", id) // Filtramos por el ID del pedido
                 .FirstOrDefaultAsync();  // Usamos FirstOrDefaultAsync para obtener un único resultado

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);  // Retornamos el modelo de tipo "pedidosinfo"
        }

        // GET: pedidos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: pedidos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_pedido,id_proveedor,fecha_pedido,id_direccion,id_metodo_pago,id_estado")] pedidos pedidos)
        {
            if (ModelState.IsValid)
            {
                // Convertir fecha_pedido a UTC si no tiene especificada la zona horaria
                if (pedidos.fecha_pedido.Kind == DateTimeKind.Unspecified)
                {
                    pedidos.fecha_pedido = DateTime.SpecifyKind(pedidos.fecha_pedido, DateTimeKind.Utc);
                }
                else
                {
                    pedidos.fecha_pedido = pedidos.fecha_pedido.ToUniversalTime();
                }

                _context.Add(pedidos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pedidos);
        }

        // GET: pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidos = await _context.pedidos.FindAsync(id);
            if (pedidos == null)
            {
                return NotFound();
            }
            return View(pedidos);
        }

        // POST: pedidos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_pedido,id_proveedor,fecha_pedido,id_direccion,id_metodo_pago,id_estado")] pedidos pedidos)
        {
            if (id != pedidos.id_pedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!pedidosExists(pedidos.id_pedido))
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
            return View(pedidos);
        }

        // GET: pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtener el pedido por el ID
            var pedido = await _context.pedidoinfos
                .FromSqlRaw(@"SELECT p.id_pedido AS ""IdPedido"",
                         pr.nombre_empresa AS ""NombreProveedor"",
                         ep.nombre_estado AS ""EstadoPedido"",
                         mp.metodo_pago AS ""MetodoPago"",
                         d.direccion_calle AS ""DireccionEntrega"",
                         d.barrio AS ""Barrio"",
                         d.ciudad AS ""Ciudad"",
                         d.codigo_postal AS ""CodigoPostal"",
                         d.pais AS ""Pais"",
                         p.fecha_pedido AS ""FechaPedido""
                  FROM pedidos p
                  JOIN proveedores pr ON p.id_proveedor = pr.id_proveedor
                  JOIN estado_pedido ep ON p.id_estado = ep.id_estado
                  LEFT JOIN metodos_pago mp ON p.id_metodo_pago = mp.id_metodo
                  JOIN direcciones d ON p.id_direccion = d.id_direccion
                  WHERE p.id_pedido = {0}", id) // Filtramos por el ID del pedido
                 .FirstOrDefaultAsync();
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);  // Pasar el pedido único a la vista
        }

        // POST: pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidos = await _context.pedidos.FindAsync(id);
            if (pedidos != null)
            {
                _context.pedidos.Remove(pedidos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool pedidosExists(int id)
        {
            return _context.pedidos.Any(e => e.id_pedido == id);
        }
    }
}
