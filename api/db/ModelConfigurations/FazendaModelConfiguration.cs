using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
  public class FazendaModelConfiguration : IEntityTypeConfiguration<Fazenda>
  {
    public virtual void Configure(EntityTypeBuilder<Fazenda> builder)
    {

    }
  }
}