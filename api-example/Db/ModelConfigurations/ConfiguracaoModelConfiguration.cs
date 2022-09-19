using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
  public class ConfiguracaoModelConfiguration : EntidadeTenantConfiguration<Configuracao>
  {
    public override void Configure(EntityTypeBuilder<Configuracao> builder)
    {
      base.Configure(builder);
    }
  }
}