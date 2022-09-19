using System.Collections.Generic;
using Alpha.Pesagem.Api.Models;
using FluentValidation;

namespace Alpha.Pesagem.Api.Validation
{
  public class SubEmpresaSaveValidator : AbstractValidator<SubEmpresa>
  {
    public SubEmpresaSaveValidator()
    {
      RuleFor(q => q.IdAlphaExpress).NotNull();
      RuleFor(q => q.Nome).MaximumLength(60).NotNull();
    }
  }
  public class SubEmpresaSaveEmLoteValidator : AbstractValidator<IEnumerable<SubEmpresa>>
  {
    public SubEmpresaSaveEmLoteValidator()
    {
      RuleForEach(q => q).SetValidator(new SubEmpresaSaveValidator());
    }
  }
}