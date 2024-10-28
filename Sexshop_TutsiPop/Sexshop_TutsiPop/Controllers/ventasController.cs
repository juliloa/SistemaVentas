using System;
using System.Collections.Generic;
using System.IO; // Asegúrate de incluir esta directiva para MemoryStream
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;
using OfficeOpenXml; // Librería para manipular Excel
using OfficeOpenXml.Style;
using Microsoft.Data.SqlClient;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using System.Configuration;


namespace Sexshop_TutsiPop.Controllers
{
    public class ventasController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public ventasController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // Acción para exportar datos a Excel
        public IActionResult ExportToExcel()
        {
            // Establecer el contexto de la licencia (NonCommercial License)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Obtener los datos de la base de datos
            var ventas = _context.ventas.ToList();

            // Crear archivo Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Ventas");

                // Definir las cabeceras
                worksheet.Cells[1, 1].Value = "ID Venta";
                worksheet.Cells[1, 2].Value = "Cédula Cliente";
                worksheet.Cells[1, 3].Value = "Cédula Empleado";
                worksheet.Cells[1, 4].Value = "Fecha Venta";
                worksheet.Cells[1, 5].Value = "ID Método Pago";

                // Establecer formato de cabeceras (opcional)
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightPink);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Agregar datos a las filas
                int row = 2;
                foreach (var venta in ventas)
                {
                    worksheet.Cells[row, 1].Value = venta.id_venta;
                    worksheet.Cells[row, 2].Value = venta.cedula_cliente;
                    worksheet.Cells[row, 3].Value = venta.cedula_empleado;
                    worksheet.Cells[row, 4].Value = venta.fecha_venta.ToUniversalTime().ToString("dd/MM/yyyy HH:mm:ss"); // Asegúrate de mostrar la fecha en UTC
                    worksheet.Cells[row, 5].Value = venta.id_metodo_pago;
                    row++;
                }

                // Ajustar ancho de las columnas
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Retornar el archivo Excel como un archivo descargable
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string excelName = $"Ventas-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        // GET: ventas
        public async Task<IActionResult> Index()
        {
            return View(await _context.ventas.ToListAsync());
        }

        // GET: ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.ventas
                .FirstOrDefaultAsync(m => m.id_venta == id);
            if (ventas == null)
            {
                return NotFound();
            }

            return View(ventas);
        }

        // GET: ventas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ventas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_venta,cedula_cliente,cedula_empleado,fecha_venta,id_metodo_pago")] ventas ventas)
        {
            if (ModelState.IsValid)
            {
                // Convertir a UTC antes de guardar
                ventas.fecha_venta = ventas.fecha_venta.ToUniversalTime();
                _context.Add(ventas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ventas);
        }

        // GET: ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.ventas.FindAsync(id);
            if (ventas == null)
            {
                return NotFound();
            }
            return View(ventas);
        }

        // POST: ventas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_venta,cedula_cliente,cedula_empleado,fecha_venta,id_metodo_pago")] ventas ventas)
        {
            if (id != ventas.id_venta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Convertir a UTC antes de guardar
                    ventas.fecha_venta = ventas.fecha_venta.ToUniversalTime();
                    _context.Update(ventas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ventasExists(ventas.id_venta))
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
            return View(ventas);
        }

        // GET: ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.ventas
                .FirstOrDefaultAsync(m => m.id_venta == id);
            if (ventas == null)
            {
                return NotFound();
            }

            return View(ventas);
        }

        // POST: ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ventas = await _context.ventas.FindAsync(id);
            if (ventas != null)
            {
                _context.ventas.Remove(ventas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ventasExists(int id)
        {
            return _context.ventas.Any(e => e.id_venta == id);
        }

    }
}
