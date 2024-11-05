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
    public class direccionesController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public direccionesController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // GET: direcciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.direcciones.ToListAsync());
        }

        // GET: direcciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direcciones = await _context.direcciones
                .FirstOrDefaultAsync(m => m.id_direccion == id);
            if (direcciones == null)
            {
                return NotFound();
            }

            return View(direcciones);
        }

        // GET: direcciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: direcciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_direccion,direccion_calle,barrio,ciudad,codigo_postal,pais")] direcciones direcciones)
        {
            if (ModelState.IsValid)
            {
                _context.Add(direcciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(direcciones);
        }

        // GET: direcciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direcciones = await _context.direcciones.FindAsync(id);
            if (direcciones == null)
            {
                return NotFound();
            }
            return View(direcciones);
        }

        // POST: direcciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_direccion,direccion_calle,barrio,ciudad,codigo_postal,pais")] direcciones direcciones)
        {
            if (id != direcciones.id_direccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(direcciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!direccionesExists(direcciones.id_direccion))
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
            return View(direcciones);
        }

        // GET: direcciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direcciones = await _context.direcciones
                .FirstOrDefaultAsync(m => m.id_direccion == id);
            if (direcciones == null)
            {
                return NotFound();
            }

            return View(direcciones);
        }

        // POST: direcciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var direcciones = await _context.direcciones.FindAsync(id);
            if (direcciones != null)
            {
                _context.direcciones.Remove(direcciones);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool direccionesExists(int id)
        {
            return _context.direcciones.Any(e => e.id_direccion == id);
        }
    }
}
