using System;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Pesagem.Api.Models
{
    public abstract class EntidadeBase
    {
        [Key]
        public Guid Id { get; set; }
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