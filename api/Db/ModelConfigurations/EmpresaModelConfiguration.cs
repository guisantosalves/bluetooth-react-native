using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
  public class EmpresaModelConfiguration : IEntityTypeConfiguration<Empresa>
  {
    public virtual void Configure(EntityTypeBuilder<Empresa> builder)
    {

    }
  }
}