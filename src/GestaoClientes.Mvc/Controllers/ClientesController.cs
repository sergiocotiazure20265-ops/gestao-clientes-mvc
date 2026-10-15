using GestaoClientes.Mvc.Data;
using GestaoClientes.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace GestaoClientes.Mvc.Controllers;
public class ClientesController(GestaoDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? busca)
    {
        ViewBag.Busca=busca;
        var q = db.Clientes.Include(x=>x.Plano).AsQueryable();
        if (!string.IsNullOrWhiteSpace(busca)) q=q.Where(x=>x.Nome.Contains(busca) || x.Cpf.Contains(busca));
        return View(await q.OrderBy(x=>x.Nome).ToListAsync());
    }
    public async Task<IActionResult> Create() { await Planos(); return View(new Cliente()); }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,Email,Cpf,PlanoId,Ativo")] Cliente model)
    {
        await Validar(model);
        if (!ModelState.IsValid) { await Planos(); return View(model); }
        db.Clientes.Add(model); await db.SaveChangesAsync();
        TempData["Mensagem"]="Cliente cadastrado com sucesso.";
        return RedirectToAction(nameof(Edit),new { id=model.Id });
    }
    public async Task<IActionResult> Edit(int id)
    {
        var cliente=await db.Clientes.FindAsync(id);
        if (cliente is null) return NotFound();
        await Planos(); return View(cliente);
    }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,[Bind("Id,Nome,Email,Cpf,PlanoId,Ativo")] Cliente model)
    {
        if (id!=model.Id) return BadRequest();
        var cliente=await db.Clientes.FindAsync(id);
        if (cliente is null) return NotFound();
        await Validar(model);
        if (!ModelState.IsValid) { await Planos(); return View(model); }
        cliente.Nome=model.Nome; cliente.Email=model.Email; cliente.Cpf=model.Cpf; cliente.PlanoId=model.PlanoId; cliente.Ativo=model.Ativo;
        await db.SaveChangesAsync(); TempData["Mensagem"]="Cliente atualizado.";
        return RedirectToAction(nameof(Index));
    }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Desativar(int id)
    {
        var cliente=await db.Clientes.FindAsync(id);
        if(cliente is null) return NotFound();
        cliente.Ativo=false; await db.SaveChangesAsync();
        TempData["Mensagem"]="Cliente desativado."; return RedirectToAction(nameof(Index));
    }
    private async Task Validar(Cliente model)
    {
        if (await db.Clientes.AnyAsync(x=>x.Cpf==model.Cpf && x.Id!=model.Id)) ModelState.AddModelError(nameof(model.Cpf),"CPF já cadastrado.");
        if (!await db.Planos.AnyAsync(x=>x.Id==model.PlanoId && x.Ativo)) ModelState.AddModelError(nameof(model.PlanoId),"O plano selecionado não existe ou está inativo.");
    }
    private async Task Planos() => ViewBag.Planos=new SelectList(await db.Planos.Where(x=>x.Ativo).OrderBy(x=>x.Nome).ToListAsync(),"Id","Nome");
}
