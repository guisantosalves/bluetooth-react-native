using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Services
{
    public class PesoService : DataService<Peso>
    {
        public PesoService(AlphaDbContext context) : base(context)
        {
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