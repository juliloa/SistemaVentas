using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sexshop_TutsiPop.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Sexshop_TutsiPopContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Sexshop_TutsiPopContext") ?? throw new InvalidOperationException("Connection string 'Sexshop_TutsiPopContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Alerta}/{id?}");



app.Run();
