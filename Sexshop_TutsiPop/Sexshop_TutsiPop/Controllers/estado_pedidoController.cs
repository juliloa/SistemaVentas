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
    public class estado_pedidoController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public estado_pedidoController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // GET: estado_pedido
        public async Task<IActionResult> Index()
        {
            return View(await _context.estado_pedido.ToListAsync());
        }

        // GET: estado_pedido/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado_pedido = await _context.estado_pedido
                .FirstOrDefaultAsync(m => m.id_estado == id);
            if (estado_pedido == null)
            {
                return NotFound();
            }

            return View(estado_pedido);
        }

        // GET: estado_pedido/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: estado_pedido/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_estado,nombre_estado")] estado_pedido estado_pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estado_pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estado_pedido);
        }

        // GET: estado_pedido/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado_pedido = await _context.estado_pedido.FindAsync(id);
            if (estado_pedido == null)
            {
                return NotFound();
            }
            return View(estado_pedido);
        }

        // POST: estado_pedido/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_estado,nombre_estado")] estado_pedido estado_pedido)
        {
            if (id != estado_pedido.id_estado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estado_pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!estado_pedidoExists(estado_pedido.id_estado))
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
            return View(estado_pedido);
        }

        // GET: estado_pedido/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado_pedido = await _context.estado_pedido
                .FirstOrDefaultAsync(m => m.id_estado == id);
            if (estado_pedido == null)
            {
                return NotFound();
            }

            return View(estado_pedido);
        }

        // POST: estado_pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estado_pedido = await _context.estado_pedido.FindAsync(id);
            if (estado_pedido != null)
            {
                _context.estado_pedido.Remove(estado_pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool estado_pedidoExists(int id)
        {
            return _context.estado_pedido.Any(e => e.id_estado == id);
        }
    }
}
