using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
  public class UsuarioModelConfiguration : EntidadeTenantConfiguration<Usuario>
  {
    public override void Configure(EntityTypeBuilder<Usuario> builder)
    {
      base.Configure(builder);
    }
  }
}