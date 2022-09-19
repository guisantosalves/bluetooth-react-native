using System.Collections.Generic;
using Alpha.Pesagem.Api.Models;
using FluentValidation;

namespace Alpha.Pesagem.Api.Validation
{
  public class UsuarioPermissaoViewModelSaveValidator : AbstractValidator<UsuarioPermissaoViewModel>
  {
    public UsuarioPermissaoViewModelSaveValidator()
    {
      
    }
  }
  public class UsuarioPermissaoViewModelSaveEmLoteValidator : AbstractValidator<IEnumerable<UsuarioPermissaoViewModel>>
  {
    public UsuarioPermissaoViewModelSaveEmLoteValidator()
    {
      RuleForEach(q => q).SetValidator(new UsuarioPermissaoViewModelSaveValidator());
    }
  }
  public class UsuarioPermissaoSaveValidator : AbstractValidator<UsuarioPermissao>
  {
    public UsuarioPermissaoSaveValidator()
    {
      
    }
  }
  public class UsuarioPermissaoSaveEmLoteValidator : AbstractValidator<IEnumerable<UsuarioPermissao>>
  {
    public UsuarioPermissaoSaveEmLoteValidator()
    {
      RuleForEach(q => q).SetValidator(new UsuarioPermissaoSaveValidator());
    }
  }
}