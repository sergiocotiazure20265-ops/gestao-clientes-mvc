using GestaoClientes.Mvc.Models;
using Microsoft.EntityFrameworkCore;
namespace GestaoClientes.Mvc.Data;
public class GestaoDbContext(DbContextOptions<GestaoDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Plano> Planos => Set<Plano>();
    public DbSet<Contrato> Contratos => Set<Contrato>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Cliente>().HasIndex(x => x.Cpf).IsUnique();
        builder.Entity<Plano>().HasIndex(x => x.Nome).IsUnique();
        builder.Entity<Plano>().Property(x => x.ValorMensal).HasPrecision(12,2);
        builder.Entity<Contrato>().Property(x => x.Valor).HasPrecision(12,2);
        builder.Entity<Cliente>().HasOne(x => x.Plano).WithMany(x => x.Clientes).HasForeignKey(x=>x.PlanoId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Contrato>().HasOne(x => x.Cliente).WithMany(x => x.Contratos).HasForeignKey(x=>x.ClienteId).OnDelete(DeleteBehavior.Restrict);
    }
}
