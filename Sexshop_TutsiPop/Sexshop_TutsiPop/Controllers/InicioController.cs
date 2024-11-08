using Sexshop_TutsiPop.Models;
using Sexshop_TutsiPop.Datos;
using Sexshop_TutsiPop.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Sexshop_TutsiPop.Data;

namespace Sexshop_TutsiPop.Controllers
{
    public class InicioController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly Dbusuarios _dbusuarios;

        public InicioController(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _dbusuarios = new Dbusuarios(configuration);
        }


        // -------------------- LOGIN --------------------

        // GET: Inicio/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Inicio/Login
        [HttpPost]
        public async Task<ActionResult> Login(string email, string contrasenna)
        {
            usuarios usuario = _dbusuarios.Validar(email, UtilidadServicio.ConvertirSHA256(contrasenna));

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
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuario.nombre),
                            new Claim(ClaimTypes.Role, usuario.rol)
                        };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (usuario.rol == "administrador")
                    {
                        return RedirectToAction("Admin", "Home");
                    }
                    else if (usuario.rol == "cliente")
                    {
                        return RedirectToAction("IndexUsuario", "Home");
                    }
                }
            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias.";
            }

            return View();
        }
        // -------------------- REGISTRAR --------------------

        // GET: Inicio/Registrar
        public ActionResult Registrar()
        {
            return View();
        }

        // POST: Inicio/Registrar
        [HttpPost]
        public ActionResult Registrar(usuarios usuario)
        {
            if (usuario.contrasenna != usuario.confirmar_contrasenna)
            {
                ViewBag.Nombre = usuario.nombre;
                ViewBag.Correo = usuario.email;
                ViewBag.Mensaje = "Las contraseñas no coinciden.";
                return View();
            }

            if (_dbusuarios.Obtener(usuario.email) == null)
            {
                usuario.contrasenna = UtilidadServicio.ConvertirSHA256(usuario.contrasenna);
                usuario.token = UtilidadServicio.GenerarToken();
                usuario.restablecer = false;
                usuario.confirmado = false;

                bool respuesta = _dbusuarios.Registrar(usuario);

                if (respuesta)
                {
                    EnviarCorreoConfirmacion(usuario);
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

        // -------------------- CONFIRMAR --------------------

        // GET: Inicio/Confirmar
        public ActionResult Confirmar(string token)
        {
            ViewBag.Respuesta = _dbusuarios.Confirmar(token);
            return View();
        }

        // -------------------- RESTABLECER --------------------

        // GET: Inicio/Restablecer
        public ActionResult Restablecer()
        {
            return View();
        }

        // POST: Inicio/Restablecer
        [HttpPost]
        public ActionResult Restablecer(string email)
        {
            usuarios usuario = _dbusuarios.Obtener(email);
            ViewBag.Correo = email;

            if (usuario != null)
            {
                bool respuesta = _dbusuarios.RestablecerActualizar(true, usuario.contrasenna, usuario.token);

                if (respuesta)
                {
                    EnviarCorreoRestablecimiento(usuario);
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

        // -------------------- ACTUALIZAR --------------------

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

            bool respuesta = _dbusuarios.RestablecerActualizar(false, UtilidadServicio.ConvertirSHA256(contrasenna), token);

            if (respuesta)
            {
                ViewBag.Restablecido = true;
            }
            else
            {
                ViewBag.Mensaje = "No se pudo actualizar.";
            }

            return View();
        }

        // -------------------- MÉTODOS PRIVADOS --------------------

        private void EnviarCorreoConfirmacion(usuarios usuario)
        {
            string path = Path.Combine(_env.ContentRootPath, "Plantilla", "Confirmar.html");
            string content = System.IO.File.ReadAllText(path);
            string url = $"{Request.Scheme}://{Request.Host}/Inicio/Confirmar?token={usuario.token}";
            string htmlbody = string.Format(content, usuario.nombre, url);

            Correo correoDTO = new Correo
            {
                Para = usuario.email,
                Asunto = "Correo confirmación",
                Contenido = htmlbody
            };

            CorreoServicio.Enviar(correoDTO);
        }

        private void EnviarCorreoRestablecimiento(usuarios usuario)
        {
            string path = Path.Combine(_env.ContentRootPath, "Plantilla", "Restablecer.html");
            string content = System.IO.File.ReadAllText(path);
            string url = $"{Request.Scheme}://{Request.Host}/Inicio/Actualizar?token={usuario.token}";
            string htmlbody = string.Format(content, usuario.nombre, url);

            Correo correoDTO = new Correo
            {
                Para = usuario.email,
                Asunto = "Restablecer cuenta",
                Contenido = htmlbody
            };

            CorreoServicio.Enviar(correoDTO);
        }
        public async Task<IActionResult> Logout()
        {
            // Cerrar sesión
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige a la vista de inicio o cualquier otra vista
            return RedirectToAction("Index", "Home");
        }
    }
}
