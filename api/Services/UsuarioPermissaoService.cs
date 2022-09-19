using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Validation;
using Alpha.Pesagem.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Services
{
    public class UsuarioPermissaoService : TenantLogDataService<UsuarioPermissao>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public UsuarioPermissaoService(AlphaDbContext context, Empresa empresa, IHttpContextAccessor httpContextAccessor) : base(context, empresa, httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task SalvarEmLoteAlphaExpressMapeadoAsync(IEnumerable<UsuarioPermissaoViewModel> list)
        {
            var usuarios = await this._context.Set<Usuario>().Where(q => list.Select(q => q.UsuarioId).Contains(q.IdAlphaExpress.Value)).ToListAsync();

            // Cria a lista mapeada trocando os ids do alpha express pelos ids Guid
            var listaFinal = new List<UsuarioPermissao>();

            foreach (var item in list)
            {
                var usuarioPermissao = new UsuarioPermissao();

                var usuario = usuarios.FirstOrDefault(q => q.IdAlphaExpress == item.UsuarioId);

                usuarioPermissao.IdAlphaExpress = item.IdAlphaExpress;
                usuarioPermissao.UsuarioId = usuario.Id;
                usuarioPermissao.AlterarValorUnitarioCompra = item.AlterarValorUnitarioCompra;
                usuarioPermissao.AlterarValorUnitarioPedido = item.AlterarValorUnitarioPedido;
                usuarioPermissao.LimiteDescontoPedido = item.LimiteDescontoPedido;

                listaFinal.Add(usuarioPermissao);
            }

            await base.SalvarEmLoteAlphaExpressAsync(listaFinal);
        }

        public async Task<UsuarioPermissao> GetPermissaoUsuarioLogadoAsync()
        {
            var usuarioId = Guid.Parse(this._context.HttpContext.User.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Sid).Value);
            var usuarioPermissao = await this._context.Set<UsuarioPermissao>().Where(q => q.UsuarioId == usuarioId).FirstOrDefaultAsync();

            return usuarioPermissao;
        }
    }
}