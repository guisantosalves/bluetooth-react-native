using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
    public class PesoModelConfiguration : IEntityTypeConfiguration<Peso>
    {
        public virtual void Configure(EntityTypeBuilder<Peso> builder)
        {
            builder.HasOne(q => q.Fazenda)
                .WithMany()
                .HasForeignKey(q => q.FazendaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}