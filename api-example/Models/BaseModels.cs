using System;

namespace Alpha.Pesagem.Api.Models
{
  public enum MbEnviar : Int16
  {
    NaoEnviar,
    Enviar,
  }
  public enum TipoPessoa : Int16
  {
    Fisica = 1,
    Juridica
  }
}