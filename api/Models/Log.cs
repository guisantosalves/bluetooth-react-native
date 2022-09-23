using System;

namespace Alpha.Pesagem.Api.Models
{
  public class Log : EntidadeBase, IAlphaExpressRef, IDateLog
  {
    public int? IdAlphaExpress { get; set; }

    public string Mensagem { get; set; }

    public Guid FazendaId { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAlteracao { get; set; }
  }
}