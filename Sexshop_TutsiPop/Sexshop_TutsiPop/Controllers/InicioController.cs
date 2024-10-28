using Sexshop_TutsiPop.Models;
using Sexshop_TutsiPop.Datos;
using Sexshop_TutsiPop.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; // Asegúrate de tener esta línea para IWebHostEnvironment

namespace Sexshop_TutsiPop.Controllers
{
    public class InicioController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public InicioController(IWebHostEnvironment env)
        {
            _env = env; // Inicializa el IWebHostEnvironment
        }

        // GET: Inicio/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Inicio/Login
        [HttpPost]
        public ActionResult Login(string email, string contrasenna)
        {
            // Validar el usuario usando el método de validación
            usuarios usuario = Dbusuarios.Validar(email, UtilidadServicio.ConvertirSHA256(contrasenna));

            if (usuario != null)
            {
                if (!usuario.confirmado)
                {
                    ViewBag.Mensaje = $"Falta confirmar su cuenta. Se le envió un correo a {email}.";
                }
                else if (usuario.restablecer)
                {
                    ViewBag.Mensaje = $"Se ha solicitado restablecer su cuenta, favor revise su bandeja del correo {email}.";
                }
                else
                {
                    
                    if (usuario.rol == "administrador")
                    {
                        return RedirectToAction("Admin", "Home"); 
                    }
                    else if (usuario.rol == "cliente")
                    {
                        return RedirectToAction("Index", "Home"); 
                    }
                }
            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias.";
            }

            return View(); // Retorna la vista de login con mensaje de error
        }

        // GET: Inicio/Registrar
        public ActionResult Registrar()
        {
            return View();
        }

        // POST: Inicio/Registrar
        [HttpPost]
        public ActionResult Registrar(usuarios usuario)
        {
            // Validar que las contraseñas coincidan
            if (usuario.contrasenna != usuario.confirmar_contrasenna)
            {
                ViewBag.Nombre = usuario.nombre;
                ViewBag.Correo = usuario.email;
                ViewBag.Mensaje = "Las contraseñas no coinciden.";
                return View();
            }

            // Verificar si el email ya está registrado
            if (Dbusuarios.Obtener(usuario.email) == null)
            {
                // Procesar el registro del nuevo usuario
                usuario.contrasenna = UtilidadServicio.ConvertirSHA256(usuario.contrasenna);
                usuario.token = UtilidadServicio.GenerarToken();
                usuario.restablecer = false;
                usuario.confirmado = false;

                bool respuesta = Dbusuarios.Registrar(usuario);

                if (respuesta)
                {
                    // Enviar correo de confirmación
                    string path = Path.Combine(_env.ContentRootPath, "Plantilla", "Confirmar.html");
                    string content = System.IO.File.ReadAllText(path);
                    string url = $"{Request.Scheme}://{Request.Host}/Inicio/Confirmar?token={usuario.token}";
                    string htmlbody = string.Format(content, usuario.nombre, url);

                    Correo correoDTO = new Correo()
                    {
                        Para = usuario.email,
                        Asunto = "Correo confirmación",
                        Contenido = htmlbody
                    };

                    bool enviado = CorreoServicio.Enviar(correoDTO);
                    ViewBag.Creado = true;
                    ViewBag.Mensaje = $"Su cuenta ha sido creada. Hemos enviado un mensaje al correo {usuario.email} para confirmar su cuenta.";
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo crear su cuenta.";
                }
            }
            else
            {
                ViewBag.Mensaje = "El correo ya se encuentra registrado.";
            }

            return View();
        }

        // GET: Inicio/Confirmar
        public ActionResult Confirmar(string token)
        {
            ViewBag.Respuesta = Dbusuarios.Confirmar(token);
            return View();
        }

        // GET: Inicio/Restablecer
        public ActionResult Restablecer()
        {
            return View();
        }

        // POST: Inicio/Restablecer
        [HttpPost]
        public ActionResult Restablecer(string email)
        {
            usuarios usuario = Dbusuarios.Obtener(email);
            ViewBag.Correo = email;

            if (usuario != null)
            {
                bool respuesta = Dbusuarios.RestablecerActualizar(true, usuario.contrasenna, usuario.token);

                if (respuesta)
                {
                    // Enviar correo para restablecer contraseña
                    string path = Path.Combine(_env.ContentRootPath, "Plantilla", "Restablecer.html");
                    string content = System.IO.File.ReadAllText(path);
                    string url = $"{Request.Scheme}://{Request.Host}/Inicio/Actualizar?token={usuario.token}";
                    string htmlbody = string.Format(content, usuario.nombre, url);

                    Correo correoDTO = new Correo()
                    {
                        Para = email,
                        Asunto = "Restablecer cuenta",
                        Contenido = htmlbody
                    };

                    bool enviado = CorreoServicio.Enviar(correoDTO);
                    ViewBag.Restablecido = true;
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo restablecer la cuenta.";
                }
            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias con el correo.";
            }

            return View();
        }

        // GET: Inicio/Actualizar
        public ActionResult Actualizar(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        // POST: Inicio/Actualizar
        [HttpPost]
        public ActionResult Actualizar(string token, string contrasenna, string confirmar_contrasenna)
        {
            ViewBag.Token = token;

            if (contrasenna != confirmar_contrasenna)
            {
                ViewBag.Mensaje = "Las contraseñas no coinciden.";
                return View();
            }

            bool respuesta = Dbusuarios.RestablecerActualizar(false, UtilidadServicio.ConvertirSHA256(contrasenna), token);

            if (respuesta)
                ViewBag.Restablecido = true;
            else
                ViewBag.Mensaje = "No se pudo actualizar.";

            return View();
        }
    }
}
