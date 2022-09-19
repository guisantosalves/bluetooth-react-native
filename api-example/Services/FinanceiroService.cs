using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Services
{
  public class FinanceiroService : TenantLogDataService<Financeiro>
  {
    public FinanceiroService(AlphaDbContext context, Empresa empresa, IHttpContextAccessor httpContextAccessor) : base(context, empresa, httpContextAccessor)
    {

    }

    public async Task SalvarEmLoteAlphaExpressMapeadoAsync(IEnumerable<FinanceiroViewModel> list)
    {
      var subEmpresas = await this._context.Set<SubEmpresa>().Where(q => list.Select(q => q.SubEmpresaId).Contains(q.IdAlphaExpress.Value)).ToListAsync();
      var clientes = await this._context.Set<Cliente>().Where(q => list.Select(q => q.ClienteId).Contains(q.IdAlphaExpress.Value)).ToListAsync();

      // Cria a lista mapeada trocando os ids do alpha express pelos ids Guid
      var listaFinal = new List<Financeiro>();

      foreach (var item in list)
      {
        var financeiro = new Financeiro();

        var subEmpresa = subEmpresas.FirstOrDefault(q => q.IdAlphaExpress == item.SubEmpresaId);
        var cliente = clientes.FirstOrDefault(q => q.IdAlphaExpress == item.ClienteId);

        financeiro.IdAlphaExpress = item.IdAlphaExpress;
        financeiro.Numero = item.Numero;
        financeiro.SubEmpresaId = subEmpresa.Id;
        financeiro.ClienteId = cliente.Id;
        financeiro.DataPagamento = item.DataPagamento;
        financeiro.DataVencimento = item.DataVencimento;
        financeiro.Situacao = item.Situacao;
        financeiro.TipoDocumento = item.TipoDocumento;
        financeiro.Valor = item.Valor;

        listaFinal.Add(financeiro);
      }

      await base.SalvarEmLoteAlphaExpressAsync(listaFinal);
    }
  }
}