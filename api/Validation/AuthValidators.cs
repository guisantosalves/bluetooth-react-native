using System;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.ViewModels;
using FluentValidation;

namespace Alpha.Pesagem.Api.Validation
{
  public class LoginValidator : AbstractValidator<LoginViewModel>
  {
    public LoginValidator()
    {
      RuleFor(q => q.Id).NotNull();
    }
  }
  public class LoginSaveValidator : AbstractValidator<Fazenda>
  {
    public LoginSaveValidator()
    {
      RuleFor(q => q.Nome).MaximumLength(60).NotNull().MinimumLength(3);
      RuleFor(q => q.Inativo).IsInEnum().NotNull();
    }
  }
}