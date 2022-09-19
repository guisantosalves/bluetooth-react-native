using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
  public class LogModelConfiguration : EntidadeTenantLogConfiguration<Log>
  {
    public override void Configure(EntityTypeBuilder<Log> builder)
    {
      base.Configure(builder);

      builder.HasOne(q => q.SubEmpresa)
      .WithMany()
      .HasForeignKey(q => q.SubEmpresaId)
      .OnDelete(DeleteBehavior.Restrict);
    }
  }
}