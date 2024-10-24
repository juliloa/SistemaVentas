using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Sexshop_TutsiPop.Data;
using Sexshop_TutsiPop.Models;

namespace Sexshop_TutsiPop.Controllers
{
    public class proveedoresController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public proveedoresController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexProveedor()
        {
            // Realizar la consulta SQL para obtener los proveedores con sus direcciones
            var proveedoresdireccion = await _context.ProveedoresDireccion
                .FromSqlRaw(@"
                    SELECT p.id_proveedor,p.nombre_empresa,p.nombre_contacto,p.numero_contacto,p.correo,p.pais,p.ciudad,p.id_direccion,p.activo, d.direccion_calle, d.barrio, d.ciudad AS CiudadDireccion, d.codigo_postal
                    FROM proveedores p
                    LEFT JOIN direcciones d ON p.id_direccion = d.id_direccion")
                .ToListAsync();

            return View("IndexProveedor", proveedoresdireccion);
        }

        // GET: proveedores
        public async Task<IActionResult> Index()
        {
            return View(await _context.proveedores.ToListAsync());
        }

        // GET: proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedores = await _context.proveedores
                .FirstOrDefaultAsync(m => m.id_proveedor == id);
            if (proveedores == null)
            {
                return NotFound();
            }

            return View(proveedores);
        }

        // GET: proveedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_proveedor,nombre_empresa,nombre_contacto,numero_contacto,correo,pais,ciudad,id_direccion,activo")] Proveedores proveedores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedores);
        }

        // GET: proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedores = await _context.proveedores.FindAsync(id);
            if (proveedores == null)
            {
                return NotFound();
            }
            return View(proveedores);
        }

        // POST: proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_proveedor,nombre_empresa,nombre_contacto,numero_contacto,correo,pais,ciudad,id_direccion,activo")] Proveedores proveedores)
        {
            if (id != proveedores.id_proveedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!proveedoresExists(proveedores.id_proveedor))
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
            return View(proveedores);
        }

        // GET: proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedores = await _context.proveedores
                .FirstOrDefaultAsync(m => m.id_proveedor == id);
            if (proveedores == null)
            {
                return NotFound();
            }

            return View(proveedores);
        }

        // POST: proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedores = await _context.proveedores.FindAsync(id);
            if (proveedores != null)
            {
                _context.proveedores.Remove(proveedores);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool proveedoresExists(int id)
        {
            return _context.proveedores.Any(e => e.id_proveedor == id);
        }
    }
}
