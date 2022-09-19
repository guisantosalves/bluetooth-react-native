using System;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Pesagem.Api.Models
{
  public class SubEmpresa : EntidadeTenant, IAlphaExpressRef, IDateLog
  {
    public int? IdAlphaExpress { get; set; }

    [StringLength(60, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Nome { get; set; }

    public MbEnviar MbEnviar { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAlteracao { get; set; }
  }
}