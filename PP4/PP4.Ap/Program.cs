using Microsoft.EntityFrameworkCore;
using PP4.Ap.Data; // 👈 Importa tu DbContext (asegúrate que el namespace coincida con el que pusiste en AppDbContext.cs)

var builder = WebApplication.CreateBuilder(args);

// 🔹 Agregar EF Core con SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Agregar controladores y vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 🔹 Configurar el pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
