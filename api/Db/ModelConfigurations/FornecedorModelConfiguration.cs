using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
  public class FornecedorModelConfiguration : EntidadeTenantLogConfiguration<Fornecedor>
  {
    public override void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
      base.Configure(builder);
    }
  }
}