using GestaoClientes.Mvc.Data;
using Microsoft.AspNetCore.Mvc;
namespace GestaoClientes.Mvc.Controllers;
public class HomeController(GestaoDbContext db) : Controller
{
    public IActionResult Index()
    {
        ViewBag.Clientes = db.Clientes.Count();
        ViewBag.Planos = db.Planos.Count();
        ViewBag.Contratos = db.Contratos.Count();
        return View();
    }
    public IActionResult Error() => View();
}
