using System;
using Alpha.Pesagem.Api.ViewModels;
using FluentValidation;

namespace Alpha.Pesagem.Api.Validation
{
  public class LoginValidator : AbstractValidator<UsuarioLoginViewModel>
  {
    public LoginValidator()
    {
      RuleFor(q => q.Id).NotNull();
      RuleFor(q => q.Senha).NotNull().MaximumLength(60).MinimumLength(4);
    }
  }
  public class UsuarioSaveValidator : AbstractValidator<Usuario>
  {
    public UsuarioSaveValidator()
    {
      RuleFor(q => q.Nome).MaximumLength(60).NotNull().MinimumLength(3);
      RuleFor(q => q.Senha).NotNull().MaximumLength(60).MinimumLength(4);
      RuleFor(q => q.IdAlphaExpress).NotNull();
      RuleFor(q => q.Inativo).IsInEnum().NotNull();
    }
  }
}