using System;

namespace Alpha.Pesagem.Api.Models
{
    public class UsuarioPermissao : EntidadeTenant, IAlphaExpressRef, IDateLog, IUsuarioPermissao
    {
        public int? IdAlphaExpress { get; set; }
        public Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public UsuarioPermissaoOpcao? AlterarValorUnitarioPedido { get; set; }
        public UsuarioPermissaoOpcao? AlterarValorUnitarioCompra { get; set; }
        public decimal? LimiteDescontoPedido { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }

    public interface IUsuarioPermissao
    {
        public int? IdAlphaExpress { get; set; }
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public UsuarioPermissaoOpcao? AlterarValorUnitarioPedido { get; set; }
        public UsuarioPermissaoOpcao? AlterarValorUnitarioCompra { get; set; }
        public decimal? LimiteDescontoPedido { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }

    public enum UsuarioPermissaoOpcao
    {
        NaoAutorizado,
        Autorizado
    }
}