using System;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Pesagem.Api.ViewModels
{
  public class UsuarioLoginViewModel
  {
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Senha { get; set; }

    [Required]
    public Guid EmpresaId { get; set; }

    [Required]
    public Guid SubEmpresaId { get; set; }
  }
  public class UsuarioSelectViewModel
  {
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Nome { get; set; }
  }
  public class TokenRefreshViewModel
  {
    [Required]
    public string ExpiredToken { get; set; }

    [Required]
    public Guid RefreshToken { get; set; }
  }
  public class TokenRefreshSubEmpresaViewModel : TokenRefreshViewModel
  {
    [Required]
    public Guid SubEmpresaId { get; set; }
  }
}