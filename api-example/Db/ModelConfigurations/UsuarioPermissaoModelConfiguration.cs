using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
    public class UsuarioPermissaoModelConfiguration : EntidadeTenantConfiguration<UsuarioPermissao>
    {
        public override void Configure(EntityTypeBuilder<UsuarioPermissao> builder)
        {
            base.Configure(builder);

            builder.HasOne(q => q.Usuario)
            .WithMany(q => q.Permissoes)
            .HasForeignKey(q => q.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}