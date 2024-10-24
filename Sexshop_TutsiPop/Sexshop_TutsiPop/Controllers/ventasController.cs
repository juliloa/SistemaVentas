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
    public class ventasController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public ventasController(Sexshop_TutsiPopContext context)
        {
            _context = context;
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_venta,cedula_cliente,cedula_empleado,fecha_venta,id_metodo_pago")] ventas ventas)
        {
            if (ModelState.IsValid)
            {
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
