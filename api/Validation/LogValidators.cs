using System.Collections.Generic;
using Alpha.Pesagem.Api.Models;
using FluentValidation;

namespace Alpha.Pesagem.Api.Validation
{
  public class LogSaveValidator : AbstractValidator<Log>
  {
    public LogSaveValidator()
    {
      RuleFor(q => q.IdAlphaExpress).NotNull();
      RuleFor(q => q.Mensagem).MaximumLength(5000).NotNull();
    }
  }
  public class LogSaveEmLoteValidator : AbstractValidator<IEnumerable<Log>>
  {
    public LogSaveEmLoteValidator()
    {
      RuleForEach(q => q).SetValidator(new LogSaveValidator());
    }
  }
}