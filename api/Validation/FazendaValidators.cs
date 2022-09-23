using System.Collections.Generic;
using Alpha.Pesagem.Api.Models;
using FluentValidation;

namespace Alpha.Pesagem.Api.Validation
{
    public class FazendaSaveValidator : AbstractValidator<Fazenda>
  {
    public FazendaSaveValidator()
    {
      RuleFor(q => q.IdAlphaExpress).NotNull();
      RuleFor(q => q.Nome).MaximumLength(60);
      RuleFor(q => q.Inativo).NotNull().IsInEnum();
    }
  }
  public class FazendaSaveEmLoteValidator : AbstractValidator<IEnumerable<Fazenda>>
  {
    public FazendaSaveEmLoteValidator()
    {
      RuleForEach(q => q).SetValidator(new FazendaSaveValidator());
    }
  }
}