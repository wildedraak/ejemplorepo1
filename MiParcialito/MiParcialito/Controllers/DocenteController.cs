using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiParcialito.Models;

namespace MiParcialito.Controllers
{
    public class DocenteController : Controller
    {
        private readonly rb100519Context _context;

        public DocenteController(rb100519Context context)
        {
            _context = context;
        }

        // GET: Docente
        public async Task<IActionResult> Index()
        {
            // Verificamos si el valor "Usuario" en la sesión está vacío (null)
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                // Si está vacío, redirigimos al método "Index" del controlador "Home"
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Si hay un valor en "Usuario", verificamos si el valor "Rol" en la sesión es "Admin"
                if (HttpContext.Session.GetString("Rol") == "Admin")
                {
                    // Si el rol es "Admin", mostramos una vista con una lista de "Docentes" desde la base de datos
                    return View(await _context.Docentes.ToListAsync());
                }
                else
                {
                    // Si el rol no es "Admin", redirigimos al método "Index" del controlador "Home"
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // GET: Docente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Docentes == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docente == null)
            {
                return NotFound();
            }

            return View(docente);
        }

        // GET: Docente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Docente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DocenteNombre")] Docente docente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(docente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(docente);
        }

        // GET: Docente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Docentes == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null)
            {
                return NotFound();
            }
            return View(docente);
        }

        // POST: Docente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DocenteNombre")] Docente docente)
        {
            if (id != docente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocenteExists(docente.Id))
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
            return View(docente);
        }

        // GET: Docente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Docentes == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docente == null)
            {
                return NotFound();
            }

            return View(docente);
        }

        // POST: Docente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Docentes == null)
            {
                return Problem("Entity set 'db100519Context.Docentes'  is null.");
            }
            var docente = await _context.Docentes.FindAsync(id);
            if (docente != null)
            {
                _context.Docentes.Remove(docente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocenteExists(int id)
        {
            return _context.Docentes.Any(e => e.Id == id);
        }
    }
}
