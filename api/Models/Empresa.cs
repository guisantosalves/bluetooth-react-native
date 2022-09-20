using System;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Pesagem.Api.Models
{
  public class Empresa : EntidadeBase, IDateLog
  {
    [StringLength(60, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Nome { get; set; }

    [StringLength(14, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Cnpj { get; set; }

    public string ImagemBase64 { get; set; }

    public bool Inativo { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAlteracao { get; set; }
  }
}