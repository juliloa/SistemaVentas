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
    public class empleadosController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public empleadosController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // GET: empleados
        public async Task<IActionResult> Index()
        {
            return View(await _context.empleados.ToListAsync());
        }

        // GET: empleados/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleados = await _context.empleados
                .FirstOrDefaultAsync(m => m.cedula_empleado == id);
            if (empleados == null)
            {
                return NotFound();
            }

            return View(empleados);
        }

        // GET: empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("cedula_empleado,nombre,apellido,correo,numero_telefono,fecha_contratacion,rol,salario,activo")] empleados empleados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleados);
        }

        // GET: empleados/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleados = await _context.empleados.FindAsync(id);
            if (empleados == null)
            {
                return NotFound();
            }
            return View(empleados);
        }

        // POST: empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("cedula_empleado,nombre,apellido,correo,numero_telefono,fecha_contratacion,rol,salario,activo")] empleados empleados)
        {
            if (id != empleados.cedula_empleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!empleadosExists(empleados.cedula_empleado))
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
            return View(empleados);
        }

        // GET: empleados/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleados = await _context.empleados
                .FirstOrDefaultAsync(m => m.cedula_empleado == id);
            if (empleados == null)
            {
                return NotFound();
            }

            return View(empleados);
        }

        // POST: empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var empleados = await _context.empleados.FindAsync(id);
            if (empleados != null)
            {
                _context.empleados.Remove(empleados);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool empleadosExists(string id)
        {
            return _context.empleados.Any(e => e.cedula_empleado == id);
        }
    }
}
