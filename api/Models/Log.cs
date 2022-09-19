using System;

namespace Alpha.Pesagem.Api.Models
{
  public class Log : EntidadeTenant, IAlphaExpressRef, IDateLog, IUsuarioLog
  {
    public int? IdAlphaExpress { get; set; }

    public string Mensagem { get; set; }

    public Guid SubEmpresaId { get; set; }

    public virtual SubEmpresa SubEmpresa { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public Guid? UsuarioCriacaoId { get; set; }

    public Usuario UsuarioCriacao { get; set; }

    public Guid? UsuarioAlteracaoId { get; set; }

    public Usuario UsuarioAlteracao { get; set; }
  }
}