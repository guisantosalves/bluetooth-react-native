using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Pesagem.Api.Data.ModelConfigurations
{
    public class LogModelConfiguration : IEntityTypeConfiguration<Log>
    {
        public virtual void Configure(EntityTypeBuilder<Log> builder)
        {
        }
    }
}