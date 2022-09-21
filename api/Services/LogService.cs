using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Services
{
  public class LogService : TenantDataService<Log>
  {
    public LogService(AlphaDbContext context, Fazenda fazenda, IHttpContextAccessor httpContextAccessor) : base(context, fazenda, httpContextAccessor)
    {

    }

    public async Task<IEnumerable<Log>> Filtrar(Guid fazendaId)
    {
      var registros = await this.Query()
        .Where(q => q.FazendaId == fazendaId)
        .ToListAsync();

      return registros;
    }

    public override async Task SalvarEmLoteAsync(IEnumerable<Log> list)
    {
      await base.SalvarEmLoteAsync(list);
      await this.RemoverLogAntigo(list.Count());
    }

    private async Task RemoverLogAntigo(int count)
    {
      var allCount = await this.Query().CountAsync();
      var oldRows = await this.Query().OrderBy(q => q.DataCriacao).Take((allCount + count) > 100 ? allCount - count : 0).ToListAsync();

      if (oldRows.Any())
      {
        this._context.RemoveRange(oldRows);
        await this._context.SaveChangesAsync();
      }
    }
  }
}