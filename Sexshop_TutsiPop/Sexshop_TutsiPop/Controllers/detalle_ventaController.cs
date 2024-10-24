using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;

namespace Sexshop_TutsiPop.Controllers
{
    public class detalle_ventaController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public detalle_ventaController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // GET: detalle_venta
        public async Task<IActionResult> Index()
        {
            return View(await _context.detalle_venta.ToListAsync());
        }

        // GET: detalle_venta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle_venta = await _context.detalle_venta
                .FirstOrDefaultAsync(m => m.id_detalle == id);
            if (detalle_venta == null)
            {
                return NotFound();
            }

            return View(detalle_venta);
        }

        // GET: detalle_venta/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: detalle_venta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_detalle,id_venta,id_producto,cantidad,descuento")] detalle_venta detalle_venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalle_venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(detalle_venta);
        }

        // GET: detalle_venta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle_venta = await _context.detalle_venta.FindAsync(id);
            if (detalle_venta == null)
            {
                return NotFound();
            }
            return View(detalle_venta);
        }

        // POST: detalle_venta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_detalle,id_venta,id_producto,cantidad,descuento")] detalle_venta detalle_venta)
        {
            if (id != detalle_venta.id_detalle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle_venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!detalle_ventaExists(detalle_venta.id_detalle))
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
            return View(detalle_venta);
        }

        // GET: detalle_venta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle_venta = await _context.detalle_venta
                .FirstOrDefaultAsync(m => m.id_detalle == id);
            if (detalle_venta == null)
            {
                return NotFound();
            }

            return View(detalle_venta);
        }

        // POST: detalle_venta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalle_venta = await _context.detalle_venta.FindAsync(id);
            if (detalle_venta != null)
            {
                _context.detalle_venta.Remove(detalle_venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool detalle_ventaExists(int id)
        {
            return _context.detalle_venta.Any(e => e.id_detalle == id);
        }
    }
}
