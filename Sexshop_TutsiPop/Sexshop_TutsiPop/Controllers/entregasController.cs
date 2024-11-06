using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;

namespace Sexshop_TutsiPop.Controllers
{
    [Authorize(Roles = "administrador")]
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
            var entregasinfo = await _context.entregasInfo
              .FromSqlRaw(@"SELECT 
                    e.id_entrega,
                    p.id_pedido,
                    d_entrega.direccion_calle AS direccion_entrega,
                    d_entrega.ciudad AS ciudad_entrega,
                    d_entrega.barrio AS barrio_entrega,
                    em.nombre AS nombre_empleado,
                    e.fecha_entrega,
                    e.estado_entrega
                FROM 
                    entregas e
                JOIN 
                    pedidos p ON e.id_pedido = p.id_pedido
                JOIN 
                    direcciones d_entrega ON e.id_direccion_entrega = d_entrega.id_direccion
                LEFT JOIN 
                    empleados em ON e.cedula_empleado = em.cedula_empleado;

                ")
              .ToListAsync();

            return View("Index", entregasinfo);
      
        }

        // GET: entregas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrega = await _context.entregasInfo
                .FromSqlRaw(@"SELECT 
                e.id_entrega,
                p.id_pedido,
                d_entrega.direccion_calle AS direccion_entrega,
                d_entrega.ciudad AS ciudad_entrega,
                d_entrega.barrio AS barrio_entrega,
                em.nombre AS nombre_empleado,
                e.fecha_entrega,
                e.estado_entrega
            FROM 
                entregas e
            JOIN 
                pedidos p ON e.id_pedido = p.id_pedido
            JOIN 
                direcciones d_entrega ON e.id_direccion_entrega = d_entrega.id_direccion
            LEFT JOIN 
                empleados em ON e.cedula_empleado = em.cedula_empleado
            WHERE e.id_entrega = {0}", id)
                .FirstOrDefaultAsync();  // Usar FirstOrDefaultAsync para obtener un solo registro

            if (entrega == null)
            {
                return NotFound();
            }

            return View("Details", entrega);  // Aquí pasas un solo objeto
        }


        // GET: entregas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: entregas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_entrega,id_pedido,id_direccion_entrega,fecha_entrega,estado_entrega,cedula_empleado")] entregas entregas)
        {
            if (ModelState.IsValid)
            {
                entregas.fecha_entrega = entregas.fecha_entrega.ToUniversalTime(); // Convertir a UTC
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
            entregas.fecha_entrega = entregas.fecha_entrega.ToUniversalTime(); // Convertir a UTC
            return View(entregas);
        }

        // POST: entregas/Edit/5
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
                    entregas.fecha_entrega = entregas.fecha_entrega.ToUniversalTime(); // Convertir a UTC
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

            var entregas = await _context.entregasInfo
                .FromSqlRaw(@"
            SELECT 
                e.id_entrega,
                p.id_pedido,
                d_entrega.direccion_calle AS direccion_entrega,
                d_entrega.ciudad AS ciudad_entrega,
                d_entrega.barrio AS barrio_entrega,
                em.nombre AS nombre_empleado,
                e.fecha_entrega,
                e.estado_entrega
            FROM 
                entregas e
            JOIN 
                pedidos p ON e.id_pedido = p.id_pedido
            JOIN 
                direcciones d_entrega ON e.id_direccion_entrega = d_entrega.id_direccion
            LEFT JOIN 
                empleados em ON e.cedula_empleado = em.cedula_empleado
            WHERE e.id_entrega = {0}", id)
                .FirstOrDefaultAsync();  // Usamos FirstOrDefaultAsync para obtener un solo resultado

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
