using Alpha.Pesagem.Api.Models;

namespace Alpha.Pesagem.Api.ViewModels
{
  public class FinanceiroViewModel : Financeiro
  {
    public new int ClienteId { get; set; }
    public new int SubEmpresaId { get; set; }
  }
}