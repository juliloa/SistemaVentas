
using Sexshop_TutsiPop.Models;
using Sexshop_TutsiPop.Datos;
using Sexshop_TutsiPop.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Sexshop_TutsiPop.Controllers
{
    public class InicioController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public InicioController(IWebHostEnvironment env)
        {
            _env = env; // Inicializa el IWebHostEnvironment
        }

        // GET: Inicio
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string contrasenna)
        {
            Usuarios usuario = DbUsuarios.Validar(email, UtilidadServicio.ConvertirSHA256(contrasenna));

            if (usuario != null)
            {
                if (!usuario.confirmado)
                {
                    ViewBag.Mensaje = $"Falta confirmar su cuenta. Se le envio un correo a {email}";
                }
                else if (usuario.restablecer)
                {
                    ViewBag.Mensaje = $"Se ha solicitado restablecer su cuenta, favor revise su bandeja del correo {email}";
                }
                else
                {
                    return RedirectToAction("Inicio", "Home");
                }

            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias";
            }


            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuarios usuario)
        {
            if (usuario.contrasenna != usuario.confirmar_contrasenna)
            {
                ViewBag.Nombre = usuario.nombre;
                ViewBag.Correo = usuario.email;
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            if (DbUsuarios.Obtener(usuario.email) == null)
            {
                usuario.contrasenna = UtilidadServicio.ConvertirSHA256(usuario.contrasenna);
                usuario.token = UtilidadServicio.GenerarToken();
                usuario.restablecer = false;
                usuario.confirmado = false;

                bool respuesta = DbUsuarios.Registrar(usuario);

                if (respuesta)
                {
                    string path = Path.Combine(_env.ContentRootPath, "Plantilla", "Confirmar.html");

                    string content = System.IO.File.ReadAllText(path);
                    string url = $"{Request.Scheme}://{Request.Host}/Inicio/Confirmar?token={usuario.token}";

                    string htmlbody = string.Format(content, usuario.nombre, url);


                    Correo correoDTO = new Correo()
                    {
                        Para = usuario.email,
                        Asunto = "Correo confirmacion",
                        Contenido = htmlbody
                    };

                    bool enviado = CorreoServicio.Enviar(correoDTO);
                    ViewBag.Creado = true;
                    ViewBag.Mensaje = $"Su cuenta ha sido creada. Hemos enviado un mensaje al correo {usuario.email} para confirmar su cuenta";
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo crear su cuenta";
                }



            }
            else
            {
                ViewBag.Mensaje = "El correo ya se encuentra registrado";
            }


            return View();
        }
        public ActionResult Confirmar(string token)
        {
            ViewBag.Respuesta = DbUsuarios.Confirmar(token);
            return View();
        }
        public ActionResult Restablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Restablecer(string email)
        {
            Usuarios usuario = DbUsuarios.Obtener(email);
            ViewBag.Correo = email;
            if (usuario != null)
            {
                bool respuesta = DbUsuarios.RestablecerActualizar(true, usuario.contrasenna, usuario.token);

                if (respuesta)
                {
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
                    ViewBag.Mensaje = "No se pudo restablecer la cuenta";
                }

            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias con el correo";
            }

            return View();
        }
        public ActionResult Actualizar(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult Actualizar(string token, string contrasenna, string confirmar_contrasenna)
        {
            ViewBag.Token = token;
            if (contrasenna != confirmar_contrasenna)
            {
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            bool respuesta = DbUsuarios.RestablecerActualizar(false, UtilidadServicio.ConvertirSHA256(contrasenna), token);

            if (respuesta)
                ViewBag.Restablecido = true;
            else
                ViewBag.Mensaje = "No se pudo actualizar";

            return View();
        }

    }
}
