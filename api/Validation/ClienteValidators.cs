using System.Collections.Generic;
using Alpha.Pesagem.Api.Models;
using FluentValidation;

namespace Alpha.Pesagem.Api.Validation
{
  public class ClienteSaveValidator : AbstractValidator<Cliente>
  {
    public ClienteSaveValidator()
    {
      RuleFor(q => q.IdAlphaExpress).NotNull();
      RuleFor(q => q.Inativo).MaximumLength(3).NotNull();
      RuleFor(q => q.Cpf).MaximumLength(14).NotNull();
      RuleFor(q => q.Rg).MaximumLength(20);
      RuleFor(q => q.NomeRazao).MaximumLength(60).NotNull();
      RuleFor(q => q.Apelido).MaximumLength(60);
      RuleFor(q => q.TipoPessoa).NotNull().IsInEnum();
      RuleFor(q => q.Sincronizado).NotNull().IsInEnum();
      RuleFor(q => q.Endereco).MaximumLength(60).NotNull();
      RuleFor(q => q.Bairro).MaximumLength(25);
      RuleFor(q => q.CodigoIbgeCidade).NotNull();
      RuleFor(q => q.Numero).MaximumLength(10);
    }
  }
  public class ClienteSaveEmLoteValidator : AbstractValidator<IEnumerable<Cliente>>
  {
    public ClienteSaveEmLoteValidator()
    {
      RuleForEach(q => q).SetValidator(new ClienteSaveValidator());
    }
  }
}