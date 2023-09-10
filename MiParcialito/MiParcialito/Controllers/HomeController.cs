using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiParcialito.Models;
using MiParcialito.repository;
using System.Diagnostics;

namespace MiParcialito.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public Encriptacion encriptacion = new Encriptacion();
        private readonly rb100519Context context;
        private readonly ILogin _loginUser;
        public  string SessionKeyUser = "Usuario";
        public  string SessionKeyRol = "Rol";
        public string SessionKeyId = "MyId";
        public HomeController(ILogger<HomeController> logger, rb100519Context context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                var modelo = await context.Materia.Include(x=>x.MateriaDocentes).ThenInclude(x=>x.Docente).ToListAsync();
                

                // var modelo = await context.AspNetUsers.Include(x => x.AspNetUserRoles).ThenInclude(x => x.Role).ToListAsync();
                return View(modelo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Login", "Home");
            }
            
        }

        [HttpPost]
        public async Task <IActionResult> LoginAccess(string username, string passcode)
        {
            //var issuccess = _loginUser.AuthenticateUser(username, passcode);
            string clave = encriptacion.GetSHA256(passcode);
            var issuccess = await context.AspNetUsers.Include(x=> x.AspNetUserRoles).ThenInclude(x=>x.Role).FirstOrDefaultAsync(authUser => authUser.Email == username && authUser.PasswordHash == clave);
          //var result    = await context.NombreDeTabla.Include(navegationProperty1).ThenInclude(navegationProperty2).Where(condiciones).FirstOrDefaultAsync(condicionDeFiltro);
           
            
            if (issuccess != null)
            {
                ViewBag.username = string.Format("Successfully logged-in", username);
                HttpContext.Session.SetString(SessionKeyUser, issuccess.Email);
                
                HttpContext.Session.SetInt32(SessionKeyId, issuccess.Id);
                
                //<td>@user.AspNetUserRoles.FirstOrDefault()?.Role.Name</td> 
                //issuccess.AspNetUserRoles.FirstOrDefault()?.Role.Name
                HttpContext.Session.SetString(SessionKeyRol, issuccess.AspNetUserRoles.FirstOrDefault()?.Role.Name);

                TempData["username"] = "test";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.username = string.Format("Login Failed ", username);
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKeyUser);
            HttpContext.Session.Remove(SessionKeyRol);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString(SessionKeyUser) == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}