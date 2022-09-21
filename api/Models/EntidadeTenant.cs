using System;

namespace Alpha.Pesagem.Api.Models
{
  public abstract class EntidadeTenant : EntidadeBase
  {
    public Guid FazendaId { get; set; }
    public Fazenda Fazenda { get; set; }
  }
}