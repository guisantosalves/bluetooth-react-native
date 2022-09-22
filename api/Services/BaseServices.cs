using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Services
{
    public class  DataService<T> : IDataService<T> where T : EntidadeBase, IDateLog, IAlphaExpressRef
    {
        protected readonly AlphaDbContext _context;
        public DataService(AlphaDbContext context)
        {
          _context = context;
        }

        public virtual async Task<T> AlterarAsync(Guid id, T obj)
        {
            var oldObj = await this.Query().AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);

            obj.Id = id;
            obj.DataCriacao = oldObj.DataCriacao;

            this._context.Update(obj);
            await this._context.SaveChangesAsync();

            return obj;
        }

        public virtual async Task<T> IncluirAsync(T obj)
        {
            this._context.Add(obj);
            await this._context.SaveChangesAsync();
            return obj;
        }

        public virtual async Task RemoverAsync(Guid id)
        {
            var obj = await this.Query().SingleOrDefaultAsync(q => q.Id == id);
            this._context.Set<T>().Remove(obj);
            await this._context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> ObterRegistrosComparadosAsync(List<ModelComparacaoViewModel> lista, int? diasAnteriores)
        {
            // Compara os registros para descobrir quem foi adicionado/alterado
            var registrosExistentes = await this.Query()
              .AsNoTracking()
              .Where(q => lista.Select(qi => qi.Id).Contains(q.Id))
              .ToListAsync();

            var registrosDesnecessarios = new List<T>();

            foreach (var item in registrosExistentes)
            {
                var itemComparacao = lista.FirstOrDefault(q => q.Id == item.Id);

                if ((item.DataAlteracao == itemComparacao.DataAlteracao) && (item.DataCriacao == itemComparacao.DataCriacao))
                {
                    registrosDesnecessarios.Add(item);
                }
            }

            var query = this.Query().AsNoTracking().AsQueryable();

            if (diasAnteriores.HasValue)
            {
                var dataAnterior = DateTime.Now;
                dataAnterior = dataAnterior.AddDays(-(diasAnteriores.Value));

                query = query.Where(q => q.DataCriacao >= dataAnterior || q.DataAlteracao >= dataAnterior);
            }

            var result = await query.ToListAsync();

            return result.Where(q => !registrosDesnecessarios.Any(d => q.Id == d.Id)).ToList();
        }

        public virtual async Task<IEnumerable<Guid>> ObterRegistrosRemovidosAsync(IEnumerable<Guid> lista)
        {
            var registrosExistentes = await this.Query().AsNoTracking().Select(q => q.Id).Where(q => lista.Contains(q)).ToListAsync();

            return lista.Except(registrosExistentes);
        }

        public virtual async Task<T> ObterUmAsync(Guid id)
        {
            return await this.Query().AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
        }

        public virtual async Task<IEnumerable<T>> ObterVariosAsync()
        {
            return await this.Query().AsNoTracking().ToListAsync();
        }

        public virtual IQueryable<T> Query()
        {
            return this._context.Set<T>().AsQueryable();
        }

        public virtual async Task<T> SalvarAsync(T obj)
        {
            if (obj.IdAlphaExpress.HasValue)
            {
                var oldObj = await this.Query().AsNoTracking().FirstOrDefaultAsync(q => q.IdAlphaExpress == obj.IdAlphaExpress.Value);

                if (oldObj == null)
                    return await this.IncluirAsync(obj);

                return await this.AlterarAsync(oldObj.Id, obj);
            }
            else
            {
                return await this.IncluirAsync(obj);
            }
        }

        public virtual async Task SalvarEmLoteAlphaExpressAsync(IEnumerable<T> list)
        {
            using (var ts = await this._context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var obj in list)
                    {
                        if (obj.IdAlphaExpress.HasValue)
                        {
                            var oldObj = await this.Query().AsNoTracking().FirstOrDefaultAsync(q => q.IdAlphaExpress == obj.IdAlphaExpress.Value);

                            if (oldObj == null)
                            {
                                await this.IncluirAsync(obj);
                                continue;
                            }

                            await this.AlterarAsync(oldObj.Id, obj);
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

        public virtual async Task SalvarEmLoteAsync(IEnumerable<T> list)
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

                            await this.AlterarAsync(oldObj.Id, obj);
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
    }
}