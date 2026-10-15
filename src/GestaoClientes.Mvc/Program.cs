using GestaoClientes.Mvc.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GestaoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestaoClientes")));

var app = builder.Build();
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// Para simplificar o laboratório inicial: cria o esquema e dados de exemplo.
// Não usar EnsureCreated em migrações reais de produção.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GestaoDbContext>();
    const int attempts = 12;
    for (var attempt = 1; attempt <= attempts; attempt++)
    {
        try { db.Database.EnsureCreated(); SeedData.Initialize(db); break; }
        catch (Microsoft.Data.SqlClient.SqlException) when (attempt < attempts)
        { Thread.Sleep(TimeSpan.FromSeconds(3)); }
    }
}
app.Run();
