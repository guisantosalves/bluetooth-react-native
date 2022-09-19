using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Services
{
  public class EmpresaService : DataService<Empresa>
  {
    public EmpresaService(AlphaDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
    {
    }

    public async Task<Empresa> ObterPorCnpjAsync(string cnpj)
    {
      return await this.Query().SingleOrDefaultAsync(q => q.Cnpj == cnpj);
    }
  }
}