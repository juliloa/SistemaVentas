using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Sexshop_TutsiPop.Data;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Sexshop_TutsiPopContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Sexshop_TutsiPopContext")
    ?? throw new InvalidOperationException("Connection string 'Sexshop_TutsiPopContext' not found.")));

// Configurar servicios de autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Inicio/Login"; // ruta de inicio de sesión
        options.LogoutPath = "/Inicio/Logout"; // ruta de cierre de sesión
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10); // Cambia el valor según lo necesario

        // Cierre de sesión automático en caso de inactividad
        options.SlidingExpiration = true;

        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.Redirect("/Home/Index");
                return Task.CompletedTask;
            }
        };
    });


   



// Agregar servicios para MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar el middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Muestra la página de detalles de excepción en desarrollo
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Redirige a la acción Error en HomeController
    app.UseHsts(); // Configuración de seguridad para HSTS en producción
}

// Middleware de manejo de rutas no encontradas (404)
app.UseStatusCodePagesWithReExecute("/Home/Error"); // Redirige a la acción Error cuando se detecta un 404

app.UseStaticFiles();  // Permite el uso de archivos estáticos

app.UseRouting();

app.UseAuthentication();  // Habilitar la autenticación
app.UseAuthorization();   // Habilitar la autorización

// Configuración de la ruta predeterminada
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Alerta}/{id?}");

app.Run();
