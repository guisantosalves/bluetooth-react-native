using System;
using System.ComponentModel.DataAnnotations;
using Alpha.Pesagem.Api.Models;

namespace Alpha.Pesagem.Api
{
    public class Usuario : EntidadeTenant, IDateLog
    {
        [StringLength(60, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Nome { get; set; }

        [StringLength(60, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Senha { get; set; }

        [StringLength(60, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Salto { get; set; }

        public int? IdAlphaExpress { get; set; }

        public UsuarioInativo Inativo { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataAlteracao { get; set; }
    }

    public enum UsuarioInativo : Int16
    {
        Ativo,
        Inativo
    }
}