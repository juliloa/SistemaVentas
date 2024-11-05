using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;

namespace Sexshop_TutsiPop.Controllers
{
    [Authorize(Roles = "administrador")]
    public class metodos_pagoController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public metodos_pagoController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // GET: metodos_pago
        public async Task<IActionResult> Index()
        {
            return View(await _context.metodos_pago.ToListAsync());
        }

        // GET: metodos_pago/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodos_pago = await _context.metodos_pago
                .FirstOrDefaultAsync(m => m.id_metodo == id);
            if (metodos_pago == null)
            {
                return NotFound();
            }

            return View(metodos_pago);
        }

        // GET: metodos_pago/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: metodos_pago/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_metodo,metodo_pago,activo")] metodos_pago metodos_pago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metodos_pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(metodos_pago);
        }

        // GET: metodos_pago/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodos_pago = await _context.metodos_pago.FindAsync(id);
            if (metodos_pago == null)
            {
                return NotFound();
            }
            return View(metodos_pago);
        }

        // POST: metodos_pago/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_metodo,metodo_pago,activo")] metodos_pago metodos_pago)
        {
            if (id != metodos_pago.id_metodo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metodos_pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!metodos_pagoExists(metodos_pago.id_metodo))
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
            return View(metodos_pago);
        }

        // GET: metodos_pago/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodos_pago = await _context.metodos_pago
                .FirstOrDefaultAsync(m => m.id_metodo == id);
            if (metodos_pago == null)
            {
                return NotFound();
            }

            return View(metodos_pago);
        }

        // POST: metodos_pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metodos_pago = await _context.metodos_pago.FindAsync(id);
            if (metodos_pago != null)
            {
                _context.metodos_pago.Remove(metodos_pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool metodos_pagoExists(int id)
        {
            return _context.metodos_pago.Any(e => e.id_metodo == id);
        }
    }
}
