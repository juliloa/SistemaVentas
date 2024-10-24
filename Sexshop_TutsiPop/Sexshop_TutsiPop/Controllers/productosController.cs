using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;

namespace Sexshop_TutsiPop.Controllers
{
    public class productosController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public productosController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

     

        // GET: productos
        public async Task<IActionResult> Index()
        {
            return View(await _context.productos.ToListAsync());
        }

        // GET: productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productos = await _context.productos
                .FirstOrDefaultAsync(m => m.id_producto == id);
            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }

        // GET: productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_producto,nombre_producto,id_categoria,id_proveedor,unidades_stock,precio,activo")] productos productos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productos);
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
            return View(productos);
        }

        // POST: productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_producto,nombre_producto,id_categoria,id_proveedor,unidades_stock,precio,activo")] productos productos)
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productos = await _context.productos
                .FirstOrDefaultAsync(m => m.id_producto == id);
            if (productos == null)
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
