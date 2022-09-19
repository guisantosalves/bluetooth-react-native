using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Services
{
  public class ClienteService : TenantLogDataService<Cliente>
  {
    public ClienteService(AlphaDbContext context, Empresa empresa, IHttpContextAccessor httpContextAccessor) : base(context, empresa, httpContextAccessor)
    {

    }

    public override async Task SalvarEmLoteAsync(IEnumerable<Cliente> list)
    {
      using (var ts = await this._context.Database.BeginTransactionAsync())
      {
        try
        {
          foreach (var obj in list)
          {
            if (obj.Id != Guid.Empty)
            {
              var oldObj = await this.Query().AsNoTracking().FirstOrDefaultAsync(q => q.Id == obj.Id);

              if (oldObj == null)
              {
                await this.IncluirAsync(obj);
              }
              else
              {
                await this.AlterarAsync(oldObj.Id, obj);
              }
            }
            else
            {
              await this.IncluirAsync(obj);
            }
          }

          await ts.CommitAsync();
        }
        catch (System.Exception)
        {
          ts.Rollback();
          throw;
        }
      }
    }

    public override async Task<IEnumerable<Cliente>> ObterRegistrosComparadosAsync(List<ModelComparacaoViewModel> lista, int? diasAnteriores)
    {
      var usuarioId = Guid.Parse(this._context.HttpContext.User.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Sid).Value);
      var usuario = await this._context.Set<Usuario>().FirstOrDefaultAsync(q => q.Id == usuarioId);

      // Compara os registros para descobrir quem foi adicionado/alterado
      var registrosExistentes = await this.Query()
        .Where(q => lista.Select(qi => qi.Id).Contains(q.Id))
        .ToListAsync();

      var registrosDesnecessarios = new List<Cliente>();

      foreach (var item in registrosExistentes)
      {
        var itemComparacao = lista.FirstOrDefault(q => q.Id == item.Id);

        if ((item.DataAlteracao == itemComparacao.DataAlteracao) && (item.DataCriacao == itemComparacao.DataCriacao))
        {
          registrosDesnecessarios.Add(item);
        }
      }

      var query = this.Query().Where(q => q.Inativo.ToLower() != "sim").AsQueryable();

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
        .Where(q => q.Inativo.ToLower() != "sim")
        .Select(q => q.Id)
        .Where(q => lista.Contains(q))
        .ToListAsync();

      return lista.Except(registrosExistentes);
    }

    public async Task<IEnumerable<Cliente>> ObterNovosAsync()
    {
      return await this.Query()
        .Where(q => q.IdAlphaExpress == 0 || q.Sincronizado == ClienteStatusSincronizado.CadastradoApp)
        .ToListAsync();
    }

    public async Task<bool> VerificarCpfOuCnpjAsync(string cpfOuCnpj)
    {
      return await this.Query()
        .Where(q => q.Cpf == cpfOuCnpj)
        .AnyAsync();
    }

    public async Task MarcarFlagSincronizadoEmLoteAsync(IEnumerable<KeyValuePair<Guid, int>> lista)
    {
      using (var ts = await this._context.Database.BeginTransactionAsync())
      {
        try
        {
          foreach (var item in lista)
          {
            var cliente = await this.Query().Where(q => q.Id == item.Key).FirstOrDefaultAsync();
            cliente.Sincronizado = ClienteStatusSincronizado.CadastradoAlpha;
            cliente.IdAlphaExpress = item.Value;

            this._context.Update(cliente);
            await this._context.SaveChangesAsync();
          }

          await ts.CommitAsync();
        }
        catch (System.Exception)
        {
          ts.Rollback();
          throw;
        }
      }
    }
  }
}