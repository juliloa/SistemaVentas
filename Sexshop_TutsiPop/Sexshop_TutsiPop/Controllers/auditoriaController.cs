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
    public class auditoriaController : Controller
    {
        private readonly Sexshop_TutsiPopContext _context;

        public auditoriaController(Sexshop_TutsiPopContext context)
        {
            _context = context;
        }

        // GET: auditoria
        public async Task<IActionResult> Index()
        {
            return View(await _context.auditoria.ToListAsync());
        }

        // GET: auditoria/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var auditoria = await _context.auditoria
        //        .FirstOrDefaultAsync(m => m.pkauditoria == id);
        //    if (auditoria == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(auditoria);
        //}

        //// GET: auditoria/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: auditoria/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("pkauditoria,nombreauditoria,tipooperacion,datosantiguos,datosnuevos,fecha,usuario")] auditoria auditoria)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(auditoria);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(auditoria);
        //}

        //// GET: auditoria/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var auditoria = await _context.auditoria.FindAsync(id);
        //    if (auditoria == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(auditoria);
        //}

        // POST: auditoria/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("pkauditoria,nombreauditoria,tipooperacion,datosantiguos,datosnuevos,fecha,usuario")] auditoria auditoria)
        //{
        //    if (id != auditoria.pkauditoria)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(auditoria);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!auditoriaExists(auditoria.pkauditoria))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(auditoria);
        //}

        //// GET: auditoria/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var auditoria = await _context.auditoria
        //        .FirstOrDefaultAsync(m => m.pkauditoria == id);
        //    if (auditoria == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(auditoria);
        //}

        //// POST: auditoria/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var auditoria = await _context.auditoria.FindAsync(id);
        //    if (auditoria != null)
        //    {
        //        _context.auditoria.Remove(auditoria);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool auditoriaExists(int id)
        {
            return _context.auditoria.Any(e => e.pkauditoria == id);
        }
    }
}
