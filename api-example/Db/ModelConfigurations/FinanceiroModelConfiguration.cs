using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
  public class FinanceiroModelConfiguration : EntidadeTenantConfiguration<Financeiro>
  {
    public override void Configure(EntityTypeBuilder<Financeiro> builder)
    {
      base.Configure(builder);

      builder.HasOne(q => q.Cliente)
      .WithMany()
      .HasForeignKey(q => q.ClienteId)
      .IsRequired(true)
      .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(q => q.SubEmpresa)
      .WithMany()
      .HasForeignKey(q => q.SubEmpresaId)
      .IsRequired(true)
      .OnDelete(DeleteBehavior.Restrict);
    }
  }
}