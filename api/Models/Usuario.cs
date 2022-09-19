using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Alpha.Pesagem.Api.Models;

namespace Alpha.Pesagem.Api
{
    public class Usuario : EntidadeTenant, IAlphaExpressRef, IDateLog
    {
        public Usuario()
        {
            this.Permissoes = new HashSet<UsuarioPermissao>();
        }

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

        [StringLength(200, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Role { get; set; }

        // Aqui são guardados os ids das subempresas que o usuário pode acessar. Cada um é separado por vírgula
        [StringLength(300, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string SubEmpresas { get; set; }
        public virtual ICollection<UsuarioPermissao> Permissoes { get; set; }
    }

    public class NivelAcesso
    {
        public const string Administrador = "Administrador";
        public const string ConsultorVendas = "Consultor de vendas";
        public const string ConsultorCompras = "Consultor de compras";
        public const string ConsultorGeral = "Consultor geral";
        public const string Todos = "Administrador, Consultor de vendas, Consultor de compras, Consultor geral";
        public const string NaoAdministradores = "Consultor geral, Consultor de vendas, Consultor de compras";
    }

    public enum UsuarioInativo : Int16
    {
        Ativo,
        Inativo
    }
}