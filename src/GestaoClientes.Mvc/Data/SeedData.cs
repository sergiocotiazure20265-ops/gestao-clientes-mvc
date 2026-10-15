using GestaoClientes.Mvc.Models;
namespace GestaoClientes.Mvc.Data;
public static class SeedData
{
    public static void Initialize(GestaoDbContext db)
    {
        if (db.Planos.Any()) return;
        var comum = new Plano { Nome="Plano Comum", ValorMensal=39.90m };
        var premium = new Plano { Nome="Plano Premium", ValorMensal=79.90m };
        var vip = new Plano { Nome="Plano VIP", ValorMensal=159.90m };
        db.Planos.AddRange(comum,premium,vip);
        db.Clientes.AddRange(
            new Cliente { Nome="Ana Souza", Email="ana@example.com", Cpf="11144477735", Plano=premium },
            new Cliente { Nome="Bruno Lima", Email="bruno@example.com", Cpf="52998224725", Plano=comum });
        db.SaveChanges();
        var ana = db.Clientes.First(x=>x.Cpf=="11144477735");
        db.Contratos.Add(new Contrato { ClienteId=ana.Id, Descricao="Contrato anual", Valor=959.00m });
        db.SaveChanges();
    }
}
