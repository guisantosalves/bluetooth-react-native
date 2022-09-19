using System.Collections.Generic;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.ViewModels;
using FluentValidation;

namespace Alpha.Pesagem.Api.Validation
{
  public class FinanceiroSaveValidator : AbstractValidator<Financeiro>
  {
    public FinanceiroSaveValidator()
    {
      RuleFor(q => q.IdAlphaExpress).NotNull();
      RuleFor(q => q.Numero).NotNull();
      RuleFor(q => q.TipoDocumento).NotNull();
      RuleFor(q => q.ClienteId).NotNull();
      RuleFor(q => q.SubEmpresaId).NotNull();
      RuleFor(q => q.Valor).NotNull();
      RuleFor(q => q.Situacao).NotNull();
    }
  }
  public class FinanceiroSaveEmLoteValidator : AbstractValidator<IEnumerable<Financeiro>>
  {
    public FinanceiroSaveEmLoteValidator()
    {
      RuleForEach(q => q).SetValidator(new FinanceiroSaveValidator());
    }
  }


  public class FinanceiroViewModelSaveValidator : AbstractValidator<FinanceiroViewModel>
  {
    public FinanceiroViewModelSaveValidator()
    {
      RuleFor(q => q.IdAlphaExpress).NotNull();
      RuleFor(q => q.Numero).NotNull();
      RuleFor(q => q.TipoDocumento).NotNull();
      RuleFor(q => q.ClienteId).GreaterThan(0).NotNull();
      RuleFor(q => q.SubEmpresaId).GreaterThan(0).NotNull();
      RuleFor(q => q.Valor).NotNull();
      RuleFor(q => q.Situacao).NotNull();
    }
  }
  public class FinanceiroViewModelSaveEmLoteValidator : AbstractValidator<IEnumerable<FinanceiroViewModel>>
  {
    public FinanceiroViewModelSaveEmLoteValidator()
    {
      RuleForEach(q => q).SetValidator(new FinanceiroViewModelSaveValidator());
    }
  }
}