using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Models;

namespace Alpha.Pesagem.Api.Services
{
  public interface IReadOnlyDataService<T> where T : EntidadeBase
  {
    Task<T> ObterUmAsync(Guid id);
    Task<IEnumerable<Guid>> ObterRegistrosRemovidosAsync(IEnumerable<Guid> lista);
    Task<IEnumerable<T>> ObterVariosAsync();
    IQueryable<T> Query();
  }

  public interface IDataService<T> : IReadOnlyDataService<T> where T : EntidadeBase
  {
    Task<T> IncluirAsync(T obj);
    Task<T> AlterarAsync(Guid id, T obj);
    Task RemoverAsync(Guid id);
    Task<IEnumerable<T>> ObterRegistrosComparadosAsync(List<ModelComparacaoViewModel> comparacao, int? diasAnteriores);
  }

  public interface ITenantDataService<T> : IDataService<T> where T : EntidadeBase
  {
    Task<T> SalvarAsync(T obj);
    Task SalvarEmLoteAlphaExpressAsync(IEnumerable<T> list);
    Task SalvarEmLoteAsync(IEnumerable<T> list);
  }
}