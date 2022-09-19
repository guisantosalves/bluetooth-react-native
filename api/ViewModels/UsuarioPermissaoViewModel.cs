using System;
using Alpha.Pesagem.Api.Models;

namespace Alpha.Pesagem.Api.Validation
{
    public class UsuarioPermissaoViewModel
    {
        public int? IdAlphaExpress { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public UsuarioPermissaoOpcao? AlterarValorUnitarioPedido { get; set; }
        public UsuarioPermissaoOpcao? AlterarValorUnitarioCompra { get; set; }
        public decimal? LimiteDescontoPedido { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}