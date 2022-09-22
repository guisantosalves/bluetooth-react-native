using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Pesagem.Api.Services
{
    public interface IDataService<T>
    {
        Task<T> ObterUmAsync(Guid id);
        Task<IEnumerable<Guid>> ObterRegistrosRemovidosAsync(IEnumerable<Guid> lista);
        Task<IEnumerable<T>> ObterVariosAsync();
        IQueryable<T> Query();
        Task<T> IncluirAsync(T obj);
        Task<T> AlterarAsync(Guid id, T obj);
        Task RemoverAsync(Guid id);
        Task<IEnumerable<T>> ObterRegistrosComparadosAsync(List<ModelComparacaoViewModel> comparacao, int? diasAnteriores);
        Task<T> SalvarAsync(T obj);
        Task SalvarEmLoteAlphaExpressAsync(IEnumerable<T> list);
        Task SalvarEmLoteAsync(IEnumerable<T> list);
    }
}