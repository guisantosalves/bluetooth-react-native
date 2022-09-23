using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Alpha.Pesagem.Api.Resolvers
{
    public static class DataServicesResolver
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IDataService<Peso>, PesoService>();
            // services.AddScoped<ITenantDataService<Fornecedor>, FornecedorService>();
            services.AddScoped<IDataService<Log>, LogService>();
            services.AddScoped<IDataService<Fazenda>, FazendaService>();
        }
    }
}