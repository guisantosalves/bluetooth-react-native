using System;

namespace Alpha.Pesagem.Api.Models
{
  public abstract class EntidadeTenant : EntidadeBase
  {
    public Guid EmpresaId { get; set; }
    public Empresa Empresa { get; set; }
  }
}