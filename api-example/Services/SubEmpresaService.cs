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
  public class SubEmpresaService : TenantDataService<SubEmpresa>
  {
    public SubEmpresaService(AlphaDbContext context, Empresa empresa, IHttpContextAccessor httpContextAccessor) : base(context, empresa, httpContextAccessor)
    {

    }
    public override async Task<IEnumerable<SubEmpresa>> ObterRegistrosComparadosAsync(List<ModelComparacaoViewModel> lista, int? diasAnteriores)
    {
      // Compara os registros para descobrir quem foi adicionado/alterado
      var registrosExistentes = await this.Query()
        .Where(q => lista.Select(qi => qi.Id).Contains(q.Id))
        .ToListAsync();

      var registrosDesnecessarios = new List<SubEmpresa>();

      foreach (var item in registrosExistentes)
      {
        var itemComparacao = lista.FirstOrDefault(q => q.Id == item.Id);

        if ((item.DataAlteracao == itemComparacao.DataAlteracao) && (item.DataCriacao == itemComparacao.DataCriacao))
        {
          registrosDesnecessarios.Add(item);
        }
      }

      var query = this.Query().Where(q => q.MbEnviar == MbEnviar.Enviar).AsQueryable();

      if (diasAnteriores.HasValue)
      {
        var dataAnterior = DateTime.Now;
        dataAnterior = dataAnterior.AddDays(-(diasAnteriores.Value));

        query = query.Where(q => q.DataCriacao >= dataAnterior || q.DataAlteracao >= dataAnterior);
      }

      var result = await query.ToListAsync();

      return result.Where(q => !registrosDesnecessarios.Any(d => q.Id == d.Id)).ToList();
    }
    public override async Task<IEnumerable<Guid>> ObterRegistrosRemovidosAsync(IEnumerable<Guid> lista)
    {
      var registrosExistentes = await this.Query()
        .Where(q => q.MbEnviar == MbEnviar.Enviar)
        .Select(q => q.Id)
        .Where(q => lista.Contains(q))
        .ToListAsync();

      return lista.Except(registrosExistentes);
    }
  }
}
