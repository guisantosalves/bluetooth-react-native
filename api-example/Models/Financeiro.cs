using System;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Pesagem.Api.Models
{
  public class Financeiro : EntidadeTenant, IAlphaExpressRef, IDateLog
  {
    public int? IdAlphaExpress { get; set; }

    [StringLength(20, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Numero { get; set; }

    [StringLength(40, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string TipoDocumento { get; set; }

    public Guid ClienteId { get; set; }

    public virtual Cliente Cliente { get; set; }

    public Guid SubEmpresaId { get; set; }

    public virtual SubEmpresa SubEmpresa { get; set; }

    public decimal Valor { get; set; }

    public DateTime? DataVencimento { get; set; }

    public DateTime? DataPagamento { get; set; }

    public string Situacao { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAlteracao { get; set; }
  }
}