using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Alpha.Pesagem.Api.Resolvers
{
    public static class DataServicesResolver
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IReadOnlyDataService<Usuario>, UsuarioService>();
            services.AddScoped<IDataService<Empresa>, EmpresaService>();
            services.AddScoped<ITenantDataService<Cliente>, ClienteService>();
            services.AddScoped<ITenantDataService<Fornecedor>, FornecedorService>();
            services.AddScoped<ITenantDataService<Log>, LogService>();
            services.AddScoped<ITenantDataService<UsuarioPermissao>, UsuarioPermissaoService>();
            services.AddScoped<ITenantDataService<SubEmpresa>, SubEmpresaService>();
            services.AddScoped<ITenantDataService<Configuracao>, TenantLogDataService<Configuracao>>();
            services.AddScoped<ITenantDataService<Financeiro>, FinanceiroService>();
        }
    }
}