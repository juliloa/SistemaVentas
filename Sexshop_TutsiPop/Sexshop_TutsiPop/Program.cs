using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Sexshop_TutsiPop.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Sexshop_TutsiPopContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Sexshop_TutsiPopContext") ?? throw new InvalidOperationException("Connection string 'Sexshop_TutsiPopContext' not found.")));

// Configurar servicios de autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Inicio/Login"; // ruta de inicio de sesión
        options.LogoutPath = "/Inicio/Logout"; // ruta de cierre de sesión
    });

// Agregar servicios para MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar el middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Agregar este middleware
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Alerta}/{id?}");

app.Run();
