using System.ComponentModel.DataAnnotations;
namespace GestaoClientes.Mvc.Models;
public class Plano
{
    public int Id { get; set; }
    [Required][StringLength(100)] public string Nome { get; set; } = "";
    [Display(Name="Valor mensal")][Range(typeof(decimal),"0.01","999999.99",ErrorMessage="Informe um valor positivo.")] public decimal ValorMensal { get; set; }
    public bool Ativo { get; set; } = true;
    public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
