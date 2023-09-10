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
    public class MateriaController : Controller
    {
        private readonly rb100519Context _context;// Reemplaza "YourDbContext" con el nombre de tu DbContext

        public MateriaController(rb100519Context context)
        {
            _context = context;
        }

        // GET: Materia
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (HttpContext.Session.GetString("Rol") == "Admin")
                {
                    return View(await _context.Materia.ToListAsync());
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // GET: Materia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Materia == null)
            {
                return NotFound();
            }

            var materium = await _context.Materia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materium == null)
            {
                return NotFound();
            }

            return View(materium);
        }

        // GET: Materia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MateriaNombre")] Materium materium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materium);
        }

        // GET: Materia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Materia == null)
            {
                return NotFound();
            }

            var materium = await _context.Materia.FindAsync(id);
            if (materium == null)
            {
                return NotFound();
            }
            return View(materium);
        }

        // POST: Materia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MateriaNombre")] Materium materium)
        {
            if (id != materium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriumExists(materium.Id))
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
            return View(materium);
        }

        // GET: Materia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Materia == null)
            {
                return NotFound();
            }

            var materium = await _context.Materia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materium == null)
            {
                return NotFound();
            }

            return View(materium);
        }

        // POST: Materia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Materia == null)
            {
                return Problem("Entity set 'db100519Context.Materia'  is null.");
            }
            var materium = await _context.Materia.FindAsync(id);
            if (materium != null)
            {
                _context.Materia.Remove(materium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /*  --------------------------- Materias disponibles para el usario XYZ --------------------------- */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> availableStudentModules([Bind("Id,MateriaNombre")] Materium materium)
        {
            if (ModelState.IsValid)
            {
                var result = await _context.AspNetUsers
                    .Where(user => user.Email == "admin@gmail.com")
                    .Select(user => user.Email)
                    .FirstOrDefaultAsync();

                // Utiliza "result" según tus necesidades

                _context.Add(materium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materium);
        }

        [HttpGet] /*  --------------------------- Materias disponibles para el usario XYZ --------------------------- */
        public async Task<IActionResult> ObtenerMaterias(int id)
        {
            // Este atributo indica que este método manejará solicitudes HTTP GET.
            // Espera un parámetro "id" que representa el ID del estudiante.
            var result = await _context.Materia
       .GroupJoin(
           _context.Inscripciones.Where(inscripcion => inscripcion.EstudianteId == id), // Se realiza un "GroupJoin" entre la tabla de "Materia" y "Inscripciones".
            // Filtra las inscripciones relacionadas con el estudiante cuyo ID es el proporcionado.

           materia => materia.Id,
           inscripcion => inscripcion.MateriaId,
           // Establece las claves de unión entre las tablas: "Id" en "Materia" y "MateriaId" en "Inscripciones".

           (materia, inscripciones) => new { Materia = materia, Inscripciones = inscripciones }
           // Crea un nuevo objeto anón
       )
       .Where(joined => !joined.Inscripciones.Any())  // Se filtran los resultados para obtener solo las materias que no tienen inscripciones relacionadas
       .Select(joined => joined.Materia.MateriaNombre)  // Se selecciona el nombre de la materia de las combinaciones resultantes
       .ToListAsync();  // Se convierte el resultado en una lista, realizando la consulta en la base de datos
                        // Aquí se realiza una consulta en la base de datos utilizando Entity Framework Core

            return View(result);// Se devuelve una vista con el resultado de la consulta para mostrar las materias que cumplen las condiciones
        }



        // GET: Materia/Create
        public IActionResult CrearMateriaEstudiante()
        {
            ViewData["Materia"] = _context.Materia.ToList();

            return View();
        }

        // POST: Materia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearMateriaEstudiante([Bind("MateriaId")] MateriumTemp materiumTemp)
        {
            if (ModelState.IsValid)
            {

                var resul = HttpContext.Session.GetInt32("MyId");
                
                Inscripcione inscripciones = new Inscripcione();
                inscripciones.EstudianteId = (int)resul;
                inscripciones.MateriaId=(int)materiumTemp.MateriaId;
                _context.Add(inscripciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(0);
        }

        public async Task<IActionResult> IndexIncripciones()
        {
            var resul = HttpContext.Session.GetInt32("MyId");
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (HttpContext.Session.GetString("Rol") == "Estudiante")
                {
                    //ViewData["MateriasInscritas"] = _context.Inscripciones.Include(x=>x.Materia).ToList().Where(f=>f.EstudianteId== resul);


                    var resultado = (from a in _context.Inscripciones
                                     join b in _context.Materia on a.MateriaId equals b.Id
                                     where a.EstudianteId == resul
                                     select new MateriaNombreViewModel
                                     {
                                         MateriaNombre = b.MateriaNombre
                                     }).ToList();

                    var listaResultado = resultado.ToList();
                    ViewData["MateriasInscritas"] = listaResultado;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        private bool MateriumExists(int id)
        {
            return _context.Materia.Any(e => e.Id == id);
        }

    }

}
