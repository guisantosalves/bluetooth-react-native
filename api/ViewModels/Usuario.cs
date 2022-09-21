using System;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Pesagem.Api.ViewModels
{
  public class LoginViewModel
  {
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid FazendaId { get; set; }
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
}