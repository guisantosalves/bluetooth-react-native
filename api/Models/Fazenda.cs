

using System;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Pesagem.Api.Models
{
  public class Fazenda : EntidadeBase, IDateLog
  {
    [StringLength(60, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Nome { get; set; }

    public FazendaInativa Inativo { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAlteracao { get; set; }
  }

  public enum FazendaInativa : Int16
    {
        Ativo,
        Inativo
    }
}