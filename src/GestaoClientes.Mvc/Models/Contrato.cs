using System.ComponentModel.DataAnnotations;
namespace GestaoClientes.Mvc.Models;
public class Contrato
{
    public int Id { get; set; }
    [Display(Name="Cliente")][Range(1,int.MaxValue,ErrorMessage="Selecione o cliente.")] public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    [Display(Name="Descrição")][Required][StringLength(180)] public string Descricao { get; set; } = "";
    [Display(Name="Valor contratado")][Range(typeof(decimal),"0.01","99999999",ErrorMessage="Informe um valor positivo.")] public decimal Valor { get; set; }
    [DataType(DataType.Date)][Display(Name="Data de início")] public DateTime DataInicio { get; set; } = DateTime.Today;
    public bool Ativo { get; set; } = true;
}
