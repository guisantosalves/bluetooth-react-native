using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
  public class ClienteModelConfiguration : EntidadeTenantLogConfiguration<Cliente>
  {
    public override void Configure(EntityTypeBuilder<Cliente> builder)
    {
      base.Configure(builder);
    }
  }
}