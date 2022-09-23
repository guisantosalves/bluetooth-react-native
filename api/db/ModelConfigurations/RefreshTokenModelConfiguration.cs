using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
  public class RefreshTokenModelConfiguration : IEntityTypeConfiguration<RefreshToken>
  {
    public virtual void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
      builder.HasOne(q => q.Fazenda)
      .WithMany()
      .HasForeignKey(q => q.FazendaId)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);
    }
  }
}