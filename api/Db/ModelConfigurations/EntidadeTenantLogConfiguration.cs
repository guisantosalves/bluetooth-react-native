using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
  public abstract class EntidadeTenantLogConfiguration<E> : EntidadeTenantConfiguration<E> where E : EntidadeTenant, IUsuarioLog
  {
    public override void Configure(EntityTypeBuilder<E> builder)
    {
      base.Configure(builder);

      builder.HasOne(q => q.UsuarioCriacao)
      .WithMany()
      .HasForeignKey(q => q.UsuarioCriacaoId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(q => q.UsuarioAlteracao)
      .WithMany()
      .HasForeignKey(q => q.UsuarioAlteracaoId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.Restrict);
    }
  }
}