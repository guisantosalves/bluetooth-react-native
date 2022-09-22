using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
    public class FornecedorModelConfiguration : IEntityTypeConfiguration<Fornecedor>
    {
        public virtual void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
        }
    }
}