using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Exceptions;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Resolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Services
{
    public class PesoService : DataService<Peso>
    {
        public PesoService(AlphaDbContext context) : base(context)
        {
        }

        public override IQueryable<Peso> Query()
        {
            return this._context.Set<Peso>()
            .AsQueryable();
        }
        public async Task<List<Peso>> ObterPesoNovasAsync()
        {
            var PesoNovas = await this.Query()
              .AsNoTracking()
              .Where(q => q.IdAlphaExpress == 0)
              .ToListAsync();

            return PesoNovas;
        }

        public async Task MarcarFlagSincronizadoEmLoteAsync(IEnumerable<KeyValuePair<Guid, int>> lista)
        {
            using (var ts = await this._context.Database.BeginTransactionAsync())
            {
                try
                {
                    var count = 1;
                    foreach (var item in lista)
                    {
                        if (item.Value == 0)
                        {
                            throw new AlphaBadRequestException($"O item {count} referente ao pedido {item.Key} está com id zerado!");
                        }

                        var pesagem = await this.Query().Where(q => q.Id == item.Key).FirstOrDefaultAsync();
                        pesagem.IdAlphaExpress = item.Value;

                        this._context.Update(pesagem);
                        await this._context.SaveChangesAsync();
                        count++;
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

        public async Task DesmarcarcarFlagSincronizadoEmLoteAsync(IEnumerable<Guid> lista)
        {
            using (var ts = await this._context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var id in lista)
                    {
                        var pesagem = await this.Query().Where(q => q.Id == id).FirstOrDefaultAsync();
                        pesagem.Sincronizado = PesagemSincronizada.Nao;
                        pesagem.IdAlphaExpress = 0;

                        this._context.Update(pesagem);
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

        public override async Task<IEnumerable<Peso>> ObterRegistrosComparadosAsync(List<ModelComparacaoViewModel> lista, int? diasAnteriores)
        {
            // Compara os registros para descobrir quem foi adicionado/alterado
            var registrosExistentes = await this.Query()
              .AsNoTracking()
              .Where(q => lista.Select(qi => qi.Id).Contains(q.Id))
              .ToListAsync();

            var registrosDesnecessarios = new List<Peso>();

            foreach (var item in registrosExistentes)
            {
                var itemComparacao = lista.FirstOrDefault(q => q.Id == item.Id);

                if ((item.DataAlteracao == itemComparacao.DataAlteracao) && (item.DataCriacao == itemComparacao.DataCriacao))
                {
                    registrosDesnecessarios.Add(item);
                }
            }

            var query = this.Query().AsNoTracking();

            if (diasAnteriores.HasValue)
            {
                var dataAnterior = DateTime.Now;
                dataAnterior = dataAnterior.AddDays(-(diasAnteriores.Value));

                query = query.Where(q => q.DataCriacao >= dataAnterior || q.DataAlteracao >= dataAnterior);
            }

            var result = await query.ToListAsync();

            return result.Where(q => !registrosDesnecessarios.Any(d => q.Id == d.Id)).ToList();
        }

        public async Task<IEnumerable<Peso>> ConsultarHistoricoAsync(DateTime dataInicial, DateTime dataFinal)
        {
            return await this.Query()
              .AsNoTracking()
              .Where(q => q.DataCriacao >= dataInicial)
              .Where(q => q.DataCriacao <= dataFinal)
              .ToListAsync();
        }
    }
}