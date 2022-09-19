using System;

namespace Alpha.Pesagem.Api.Models
{
  public class RefreshToken : EntidadeBase, IDateLog
  {
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
    public Guid Token { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
  }
}