using System;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Pesagem.Api.Models
{
  public abstract class EntidadeBase
  {
    [Key]
    public Guid Id { get; set; }
  }

  public interface IUsuarioLog
  {
    Guid? UsuarioCriacaoId { get; set; }
    Usuario UsuarioCriacao { get; set; }
    Guid? UsuarioAlteracaoId { get; set; }
    Usuario UsuarioAlteracao { get; set; }
  }

  public interface IDateLog
  {
    DateTime DataCriacao { get; set; }
    DateTime? DataAlteracao { get; set; }
  }

  public interface IAlphaExpressRef
  {
    int? IdAlphaExpress { get; set; }
  }
}