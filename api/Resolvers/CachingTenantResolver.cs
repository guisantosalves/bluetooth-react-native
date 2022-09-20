

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SaasKit.Multitenancy;

namespace Alpha.Pesagem.Api.Resolvers
{
    public class CachingTenantResolver : MemoryCacheTenantResolver<Empresa>
    {
        private readonly AlphaDbContext _context;

        public CachingTenantResolver(AlphaDbContext context, IMemoryCache cache, ILoggerFactory loggerFactory) : base(cache, loggerFactory)
        {
            _context = context;
        }

        // Resolver runs on cache misses
        protected override async Task<TenantContext<Empresa>> ResolveAsync(HttpContext context)
        {
            //Se não está autenticado, não tem claim, portanto não tem tenant
            if (!context.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var claim = context.User.Claims.FirstOrDefault(c => c.Type.Equals(AlphaClaimTypes.TenantId));
            if (claim == null)
            {
                return null;
            }

            var empresa = await _context.Set<Empresa>().IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(e => e.Id == Guid.Parse(claim.Value));

            if (empresa == null) return null;

            return new TenantContext<Empresa>(empresa);
        }

        protected override MemoryCacheEntryOptions CreateCacheEntryOptions() => new MemoryCacheEntryOptions().SetSize(512000).SetAbsoluteExpiration(TimeSpan.FromHours(2));

        protected override string GetContextIdentifier(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated) return null;
            return context.User.Claims.FirstOrDefault(c => c.Type.Equals(AlphaClaimTypes.TenantId)).Value;
        }

        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<Empresa> context) => new string[] { context.Tenant.Id.ToString() };
    }

    public static class AlphaClaimTypes
    {
        public const string TenantId = "TenantId";

        public const string TenantNome = "TenantNome";
    }

}