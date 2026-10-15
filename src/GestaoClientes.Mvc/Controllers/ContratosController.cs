using GestaoClientes.Mvc.Data;
using GestaoClientes.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace GestaoClientes.Mvc.Controllers;
public class ContratosController(GestaoDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Contratos.Include(x=>x.Cliente).ThenInclude(x=>x!.Plano).OrderByDescending(x=>x.DataInicio).ToListAsync());
    public async Task<IActionResult> Create() { await Clientes(); return View(new Contrato()); }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ClienteId,Descricao,Valor,DataInicio,Ativo")] Contrato model)
    {
        if (!await db.Clientes.AnyAsync(x=>x.Id==model.ClienteId && x.Ativo)) ModelState.AddModelError(nameof(model.ClienteId),"Selecione um cliente ativo.");
        if(!ModelState.IsValid) { await Clientes(); return View(model); }
        db.Contratos.Add(model); await db.SaveChangesAsync();
        TempData["Mensagem"]="Contrato cadastrado."; return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id)
    {
        var contrato=await db.Contratos.FindAsync(id); if(contrato is null) return NotFound();
        await Clientes(); return View(contrato);
    }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,[Bind("Id,ClienteId,Descricao,Valor,DataInicio,Ativo")] Contrato model)
    {
        if(id!=model.Id) return BadRequest();
        var contrato=await db.Contratos.FindAsync(id); if(contrato is null) return NotFound();
        if(!await db.Clientes.AnyAsync(x=>x.Id==model.ClienteId && x.Ativo)) ModelState.AddModelError(nameof(model.ClienteId),"Selecione um cliente ativo.");
        if(!ModelState.IsValid) { await Clientes(); return View(model); }
        contrato.ClienteId=model.ClienteId; contrato.Descricao=model.Descricao; contrato.Valor=model.Valor; contrato.DataInicio=model.DataInicio; contrato.Ativo=model.Ativo;
        await db.SaveChangesAsync(); TempData["Mensagem"]="Contrato atualizado."; return RedirectToAction(nameof(Index));
    }
    private async Task Clientes() => ViewBag.Clientes=new SelectList(await db.Clientes.Where(x=>x.Ativo).OrderBy(x=>x.Nome).ToListAsync(),"Id","Nome");
}
