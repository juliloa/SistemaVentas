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
    public class entregasController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public entregasController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // GET: entregas
        public async Task<IActionResult> Index()
        {
            return View(await _context.entregas.ToListAsync());
        }

        // GET: entregas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entregas = await _context.entregas
                .FirstOrDefaultAsync(m => m.id_entrega == id);
            if (entregas == null)
            {
                return NotFound();
            }

            return View(entregas);
        }

        // GET: entregas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: entregas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_entrega,id_pedido,id_direccion_entrega,fecha_entrega,estado_entrega,cedula_empleado")] entregas entregas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entregas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entregas);
        }

        // GET: entregas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entregas = await _context.entregas.FindAsync(id);
            if (entregas == null)
            {
                return NotFound();
            }
            return View(entregas);
        }

        // POST: entregas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_entrega,id_pedido,id_direccion_entrega,fecha_entrega,estado_entrega,cedula_empleado")] entregas entregas)
        {
            if (id != entregas.id_entrega)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entregas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!entregasExists(entregas.id_entrega))
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
            return View(entregas);
        }

        // GET: entregas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entregas = await _context.entregas
                .FirstOrDefaultAsync(m => m.id_entrega == id);
            if (entregas == null)
            {
                return NotFound();
            }

            return View(entregas);
        }

        // POST: entregas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entregas = await _context.entregas.FindAsync(id);
            if (entregas != null)
            {
                _context.entregas.Remove(entregas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool entregasExists(int id)
        {
            return _context.entregas.Any(e => e.id_entrega == id);
        }
    }
}
