using System.ComponentModel.DataAnnotations;
namespace GestaoClientes.Mvc.Models;
public class Cliente
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Informe o nome.")][StringLength(150)] public string Nome { get; set; } = "";
    [Required(ErrorMessage = "Informe o e-mail.")][EmailAddress][StringLength(150)] public string Email { get; set; } = "";
    [Required(ErrorMessage = "Informe o CPF.")][RegularExpression(@"^\d{11}$", ErrorMessage="O CPF deve ter 11 dígitos.")] public string Cpf { get; set; } = "";
    [Display(Name="Plano")][Range(1,int.MaxValue,ErrorMessage="Selecione um plano.")] public int PlanoId { get; set; }
    public Plano? Plano { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
}
