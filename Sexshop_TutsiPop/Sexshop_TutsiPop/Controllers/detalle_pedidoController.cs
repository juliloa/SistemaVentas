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
    public class detalle_pedidoController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public detalle_pedidoController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // GET: detalle_pedido
        public async Task<IActionResult> Index()
        {
            var detallepedidoinfo = await _context.detallepedidoInfo
                .FromSqlRaw(@"SELECT 
                        dp.id_detalle,
                        dp.id_pedido,
                        p.nombre_producto,
                        pr.nombre_empresa AS proveedor,
                        dp.cantidad,
                        dp.descuento,
                        ped.fecha_pedido,
                        e.nombre_estado AS estado
                    FROM 
                        detalle_pedido dp
                    JOIN 
                        pedidos ped ON dp.id_pedido = ped.id_pedido
                    JOIN 
                        productos p ON dp.id_producto = p.id_producto
                    JOIN 
                        proveedores pr ON p.id_proveedor = pr.id_proveedor
                    JOIN 
                        estado_pedido e ON ped.id_estado = e.id_estado;
                    ")
                .ToListAsync();

            return View("Index", detallepedidoinfo);
            //return View(await _context.detalle_pedido.ToListAsync());
        }

        // GET: detalle_pedido/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle_pedido = await _context.detalle_pedido
                .FirstOrDefaultAsync(m => m.id_detalle == id);
            if (detalle_pedido == null)
            {
                return NotFound();
            }

            return View(detalle_pedido);
        }

        // GET: detalle_pedido/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: detalle_pedido/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_detalle,id_pedido,id_producto,cantidad,descuento")] detalle_pedido detalle_pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalle_pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(detalle_pedido);
        }

        // GET: detalle_pedido/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle_pedido = await _context.detalle_pedido.FindAsync(id);
            if (detalle_pedido == null)
            {
                return NotFound();
            }
            return View(detalle_pedido);
        }

        // POST: detalle_pedido/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_detalle,id_pedido,id_producto,cantidad,descuento")] detalle_pedido detalle_pedido)
        {
            if (id != detalle_pedido.id_detalle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle_pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!detalle_pedidoExists(detalle_pedido.id_detalle))
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
            return View(detalle_pedido);
        }

        // GET: detalle_pedido/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle_pedido = await _context.detalle_pedido
                .FirstOrDefaultAsync(m => m.id_detalle == id);
            if (detalle_pedido == null)
            {
                return NotFound();
            }

            return View(detalle_pedido);
        }

        // POST: detalle_pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalle_pedido = await _context.detalle_pedido.FindAsync(id);
            if (detalle_pedido != null)
            {
                _context.detalle_pedido.Remove(detalle_pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool detalle_pedidoExists(int id)
        {
            return _context.detalle_pedido.Any(e => e.id_detalle == id);
        }
    }
}
