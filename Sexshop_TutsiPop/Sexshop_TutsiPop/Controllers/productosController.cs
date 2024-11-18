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
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Sexshop_TutsiPop.Controllers
{
    [Authorize]
    public class productosController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public productosController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // Acción para exportar datos a Excel
        public IActionResult ExportToExcel()
        {
            // Establecer el contexto de la licencia (NonCommercial License)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Obtener los datos de la base de datos
            var Productos = _context.productos.ToList();

            // Crear archivo Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("productosInfo");

                // Definir las cabeceras
                worksheet.Cells[1, 1].Value = "Nombre Producto";
                worksheet.Cells[1, 2].Value = "Unidades Stock";
                worksheet.Cells[1, 3].Value = "Precio";


                // Establecer formato de cabeceras (opcional)
                using (var range = worksheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightPink);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Agregar datos a las filas
                int row = 2;
                foreach (var productos in Productos)
                {
                    worksheet.Cells[row, 1].Value = productos.nombre_producto;
                    worksheet.Cells[row, 2].Value = productos.unidades_stock;
                    worksheet.Cells[row, 3].Value = productos.precio;

                    row++;
                }

                // Ajustar ancho de las columnas
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Retornar el archivo Excel como un archivo descargable
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string excelName = $"Productos-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }


        // GET: productos
         [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Index()
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

            return View("Index", productosinfo);
        }



        // GET: productos/Details/5
        // GET: productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productos = await _context.productosInfo
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
                        proveedores pr ON p.id_proveedor = pr.id_proveedor
                    WHERE 
                        p.id_producto = {0}", id)
                .FirstOrDefaultAsync();

            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }


        // GET: productos/Create
        // GET: productos/Create
        public IActionResult Create()
        {
         
            var categorias = _context.categorias
                .Select(c => new { c.id_categoria, c.nombre_categoria })
                .ToList();

            var proveedores = _context.proveedores
                .Select(p => new { p.id_proveedor, p.nombre_empresa })
                .ToList();

            // Si estas listas son nulas o están vacías, arroja una excepción
            if (categorias == null || !categorias.Any())
            {
                throw new Exception("No se encontraron categorías en la base de datos.");
            }
            if (proveedores == null || !proveedores.Any())
            {
                throw new Exception("No se encontraron proveedores en la base de datos.");
            }

            // Pasar las listas a la vista a través de ViewBag
            ViewBag.Categorias = new SelectList(categorias, "id_categoria", "nombre_categoria");
            ViewBag.Proveedores = new SelectList(proveedores, "id_proveedor", "nombre_empresa");

            return View();
        }



        // POST: productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(productos producto)
        {
            if (ModelState.IsValid)
            {
                _context.productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Volver a cargar las listas en caso de que haya un error de validación
            ViewBag.Categorias = new SelectList(_context.categorias, "id_categoria", "nombre_categoria", producto.id_categoria);
            ViewBag.Proveedores = new SelectList(_context.proveedores, "id_proveedor", "nombre_empresa", producto.id_proveedor);

            return View(producto);
        }


        // GET: productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productos = await _context.productos.FindAsync(id);
            if (productos == null)
            {
                return NotFound();
            }

            // Cargar las categorías y proveedores
            var categorias = _context.categorias
                .Select(c => new { c.id_categoria, c.nombre_categoria })
                .ToList();

            var proveedores = _context.proveedores
                .Select(p => new { p.id_proveedor, p.nombre_empresa })
                .ToList();

            // Pasar las listas a la vista a través de ViewBag
            ViewBag.Categorias = new SelectList(categorias, "id_categoria", "nombre_categoria", productos.id_categoria);
            ViewBag.Proveedores = new SelectList(proveedores, "id_proveedor", "nombre_empresa", productos.id_proveedor);

            return View(productos);
        }


        // POST: productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_producto,nombre_producto,id_categoria,id_proveedor,unidades_stock,precio,activo,imagen_url")] productos productos)
        {
            

            if (id != productos.id_producto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!productosExists(productos.id_producto))
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
            return View(productos);
        }

        // GET: productos/Delete/5
        // GET: productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productos = await _context.productosInfo
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
                        proveedores pr ON p.id_proveedor = pr.id_proveedor
                    WHERE 
                        p.id_producto = {0}"
            , id)
            .FirstOrDefaultAsync();

            if (productos == null )
            {
                return NotFound();
            }

            return View(productos);
        }


        // POST: productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productos = await _context.productos.FindAsync(id);
            if (productos != null)
            {
                _context.productos.Remove(productos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool productosExists(int id)
        {
            return _context.productos.Any(e => e.id_producto == id);
        }
    }
}
