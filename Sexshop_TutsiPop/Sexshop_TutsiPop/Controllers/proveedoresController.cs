using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;
using Microsoft.AspNetCore.Authorization;

namespace Sexshop_TutsiPop.Controllers
{
    [Authorize(Roles = "administrador")]
    public class proveedoresController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public proveedoresController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }


        // Acción para exportar datos a Excel
        public IActionResult ExportToExcel()
        {
            // Establecer el contexto de la licencia (NonCommercial License)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Obtener los datos de la base de datos
            var Proveedores = _context.proveedores.ToList();

            // Crear archivo Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Proveedores");

                // Definir las cabeceras
                worksheet.Cells[1, 1].Value = "ID Proveedor";
                worksheet.Cells[1, 2].Value = "Nombre Empresa";
                worksheet.Cells[1, 3].Value = "Nombre Contacto";
                worksheet.Cells[1, 4].Value = "Correo";
                worksheet.Cells[1, 5].Value = "Pais";
                worksheet.Cells[1, 6].Value = "Ciudad";

                // Establecer formato de cabeceras (opcional)
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightPink);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Agregar datos a las filas
                int row = 2;
                foreach (var proveedores in Proveedores)
                {
                    worksheet.Cells[row, 1].Value = proveedores.id_proveedor;
                    worksheet.Cells[row, 2].Value = proveedores.nombre_empresa;
                    worksheet.Cells[row, 3].Value = proveedores.nombre_contacto;
                    worksheet.Cells[row, 4].Value = proveedores.correo;
                    worksheet.Cells[row, 5].Value = proveedores.pais;
                    worksheet.Cells[row, 6].Value = proveedores.ciudad;

                    row++;
                }

                // Ajustar ancho de las columnas
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Retornar el archivo Excel como un archivo descargable
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string excelName = $"Proveedores-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
        public async Task<IActionResult> IndexProveedor()
        {
            // Realizar la consulta SQL para obtener los proveedores con sus direcciones
            var proveedoresdireccion = await _context.ProveedoresDireccion
                .FromSqlRaw(@"
                    SELECT p.id_proveedor,p.nombre_empresa,p.nombre_contacto,p.numero_contacto,p.correo,p.pais,p.ciudad,p.activo, d.direccion_calle, d.barrio, d.ciudad AS CiudadDireccion, d.codigo_postal
                    FROM proveedores p
                    LEFT JOIN direcciones d ON p.id_direccion = d.id_direccion")
                .ToListAsync();

            return View("IndexProveedor", proveedoresdireccion);
        }

        // GET: proveedores
        public async Task<IActionResult> Index()
        {
            return View(await _context.proveedores.ToListAsync());
        }

        // GET: proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedores = await _context.proveedores
                .FirstOrDefaultAsync(m => m.id_proveedor == id);
            if (proveedores == null)
            {
                return NotFound();
            }

            return View(proveedores);
        }

        // GET: proveedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_proveedor,nombre_empresa,nombre_contacto,numero_contacto,correo,pais,ciudad,id_direccion,activo")] Proveedores proveedores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedores);
        }

        // GET: proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedores = await _context.proveedores.FindAsync(id);
            if (proveedores == null)
            {
                return NotFound();
            }
            return View(proveedores);
        }

        // POST: proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_proveedor,nombre_empresa,nombre_contacto,numero_contacto,correo,pais,ciudad,id_direccion,activo")] Proveedores proveedores)
        {
            if (id != proveedores.id_proveedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!proveedoresExists(proveedores.id_proveedor))
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
            return View(proveedores);
        }

        // GET: proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedores = await _context.proveedores
                .FirstOrDefaultAsync(m => m.id_proveedor == id);
            if (proveedores == null)
            {
                return NotFound();
            }

            return View(proveedores);
        }

        // POST: proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedores = await _context.proveedores.FindAsync(id);
            if (proveedores != null)
            {
                _context.proveedores.Remove(proveedores);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool proveedoresExists(int id)
        {
            return _context.proveedores.Any(e => e.id_proveedor == id);
        }
    }
}
