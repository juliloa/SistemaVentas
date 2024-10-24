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
    public class pedidosController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public pedidosController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // GET: pedidos
        public async Task<IActionResult> Index()
        {
            return View(await _context.pedidos.ToListAsync());
        }

        // GET: pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidos = await _context.pedidos
                .FirstOrDefaultAsync(m => m.id_pedido == id);
            if (pedidos == null)
            {
                return NotFound();
            }

            return View(pedidos);
        }

        // GET: pedidos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_pedido,id_proveedor,fecha_pedido,id_direccion,id_metodo_pago,id_estado")] pedidos pedidos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedidos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pedidos);
        }

        // GET: pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidos = await _context.pedidos.FindAsync(id);
            if (pedidos == null)
            {
                return NotFound();
            }
            return View(pedidos);
        }

        // POST: pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_pedido,id_proveedor,fecha_pedido,id_direccion,id_metodo_pago,id_estado")] pedidos pedidos)
        {
            if (id != pedidos.id_pedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!pedidosExists(pedidos.id_pedido))
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
            return View(pedidos);
        }

        // GET: pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidos = await _context.pedidos
                .FirstOrDefaultAsync(m => m.id_pedido == id);
            if (pedidos == null)
            {
                return NotFound();
            }

            return View(pedidos);
        }

        // POST: pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidos = await _context.pedidos.FindAsync(id);
            if (pedidos != null)
            {
                _context.pedidos.Remove(pedidos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool pedidosExists(int id)
        {
            return _context.pedidos.Any(e => e.id_pedido == id);
        }
    }
}
