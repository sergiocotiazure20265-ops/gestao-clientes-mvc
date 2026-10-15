using GestaoClientes.Mvc.Data;
using GestaoClientes.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace GestaoClientes.Mvc.Controllers;
public class PlanosController(GestaoDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Planos.OrderBy(x=>x.Nome).ToListAsync());
    public IActionResult Create() => View(new Plano());
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,ValorMensal,Ativo")] Plano model)
    {
        if(await db.Planos.AnyAsync(x=>x.Nome==model.Nome)) ModelState.AddModelError(nameof(model.Nome),"Nome já cadastrado.");
        if(!ModelState.IsValid) return View(model);
        db.Planos.Add(model); await db.SaveChangesAsync();
        TempData["Mensagem"]="Plano cadastrado."; return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id) => await db.Planos.FindAsync(id) is { } plano ? View(plano) : NotFound();
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,[Bind("Id,Nome,ValorMensal,Ativo")] Plano model)
    {
        if(id!=model.Id) return BadRequest();
        var plano=await db.Planos.FindAsync(id); if(plano is null) return NotFound();
        if(await db.Planos.AnyAsync(x=>x.Nome==model.Nome && x.Id!=id)) ModelState.AddModelError(nameof(model.Nome),"Nome já cadastrado.");
        if(!model.Ativo && await db.Clientes.AnyAsync(x=>x.PlanoId==id && x.Ativo)) ModelState.AddModelError(nameof(model.Ativo),"Não é possível desativar plano com clientes ativos.");
        if(!ModelState.IsValid) return View(model);
        plano.Nome=model.Nome; plano.ValorMensal=model.ValorMensal; plano.Ativo=model.Ativo;
        await db.SaveChangesAsync(); TempData["Mensagem"]="Plano atualizado."; return RedirectToAction(nameof(Index));
    }
}
