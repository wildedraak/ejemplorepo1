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
    public class AspNetUsersController : Controller
    {
        private readonly rb100519Context _context;
        public Encriptacion encriptacion= new Encriptacion();

        public AspNetUsersController(rb100519Context context)
        {
            _context = context;
        }

        // GET: AspNetUsers
        public async Task<IActionResult> Index()
        {
              return View(await _context.AspNetUsers.ToListAsync());
        }

        // GET: AspNetUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AspNetUsers == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        public IActionResult Create()
        {
            ViewData["Roles"] = _context.AspNetRoles.ToList();
            return View();
        }

        


        // POST: AspNetUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,PasswordHash,Edad,RolId")] UsuarioTemp usuarioTemp)
        {
            if (ModelState.IsValid)
            {
                AspNetUser aspNetUser = new AspNetUser();
               // aspNetUser.Id = usuarioTemp.Id;

                aspNetUser.UserName = usuarioTemp.UserName;
                aspNetUser.Email = usuarioTemp.Email;
                string clave =encriptacion.GetSHA256(usuarioTemp.PasswordHash);
                aspNetUser.PasswordHash =  clave;
                aspNetUser.Edad = usuarioTemp.Edad;
                //usuarioTemp.RolId;
                _context.Add(aspNetUser);

                await _context.SaveChangesAsync();
                int nuevoId = aspNetUser.Id;
                AspNetUserRole aspNetUserRole = new AspNetUserRole();
                aspNetUserRole.UserId = nuevoId;
                aspNetUserRole.RoleId = (int)usuarioTemp.RolId; // Asegúrate de que este valor sea el correcto para el rol deseado

                // Agregar y guardar el objeto AspNetUserRole
                _context.Add(aspNetUserRole);

                
                await _context.SaveChangesAsync();

                

                return RedirectToAction(nameof(Index));
            }
            return View(usuarioTemp);
        }

        // GET: AspNetUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AspNetUsers == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Email,PasswordHash,Edad")] AspNetUser aspNetUser)
        {
            if (id != aspNetUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserExists(aspNetUser.Id))
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
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AspNetUsers == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AspNetUsers == null)
            {
                return Problem("Entity set 'rb100519Context.AspNetUsers'  is null.");
            }
            var aspNetUser = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUser != null)
            {
                _context.AspNetUsers.Remove(aspNetUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateEstudiante()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEstudiante([Bind("Id,UserName,Email,PasswordHash,Edad")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUser);
                await _context.SaveChangesAsync();

                // Después de llamar a SaveChangesAsync(), el ID generado se encuentra en la propiedad "Id" de la entidad aspNetUser
                int nuevoId = aspNetUser.Id;
                AspNetUserRole aspNetUserRole = new AspNetUserRole();
                aspNetUserRole.UserId = nuevoId;
                aspNetUserRole.RoleId = 2; // Asegúrate de que este valor sea el correcto para el rol deseado

                // Agregar y guardar el objeto AspNetUserRole
                _context.Add(aspNetUserRole);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(aspNetUser);
        }

        // Maneja la solicitud POST cuando se envían datos desde el formulario de creación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> disponibilidadMateria([Bind("Id,MateriaNombre")] Materium materium, string correo)
        {
            if (ModelState.IsValid) // Verifica si los datos enviados son válidos
            {
                // Realiza la consulta para obtener el correo electrónico del administrador
                var result = await _context.AspNetUsers
                    .Where(user => user.Email == correo)
                    .Select(user => user.Email)
                    .FirstOrDefaultAsync();

                // Utiliza "result" según tus necesidades, por ejemplo, puedes guardarlo en una variable para usarlo después

                _context.Add(materium); // Agrega la nueva materia a la base de datos
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
                return RedirectToAction(nameof(Index)); // Redirige a la acción "Index" después de guardar

            }

            // Si los datos no son válidos, vuelve a mostrar la vista de creación con los errores de validación
            return View(materium);
        }

        private bool AspNetUserExists(int id)
        {
          return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
