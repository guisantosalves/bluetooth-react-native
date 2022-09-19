using System.Collections.Generic;
using Alpha.Pesagem.Api.Models;
using FluentValidation;

namespace Alpha.Pesagem.Api.Validation
{
  public class ConfiguracaoSaveValidator : AbstractValidator<Configuracao>
  {
    public ConfiguracaoSaveValidator()
    {
      RuleFor(q => q.IdAlphaExpress).NotNull();
      RuleFor(q => q.UsarTabelaPrecos).NotNull();
      RuleFor(q => q.RestringirEstoque).NotNull();
      RuleFor(q => q.AlterarClientes).NotNull();
      RuleFor(q => q.ArrastarParaAdicionar).NotNull();
      RuleFor(q => q.ClientePorRegiao).NotNull();
      RuleFor(q => q.AlterarValorProduto).NotNull();
      RuleFor(q => q.CadastrarClientes).NotNull();
      RuleFor(q => q.AdicionarAcrescimos).NotNull();
      RuleFor(q => q.AdicionarAcrescimoVenda).NotNull();
      RuleFor(q => q.BloquearImpressaoPdf).NotNull();
      RuleFor(q => q.AplicaDescontoComTabelaPreco).NotNull();
      RuleFor(q => q.DescontoMaximo).NotNull();
      RuleFor(q => q.AcrescimoMaximo).NotNull();
      RuleFor(q => q.PeriodoPadraoEmListagens).NotNull();
      RuleFor(q => q.AplicarDesconto).NotNull();
      RuleFor(q => q.OcultarDashboard).NotNull();
      RuleFor(q => q.AplicarValorAtacado).NotNull();
      RuleFor(q => q.AplicarQtdMinimaAtacado).NotNull();
    }
  }
  public class ConfiguracaoSaveEmLoteValidator : AbstractValidator<IEnumerable<Configuracao>>
  {
    public ConfiguracaoSaveEmLoteValidator()
    {
      RuleForEach(q => q).SetValidator(new ConfiguracaoSaveValidator());
    }
  }
}