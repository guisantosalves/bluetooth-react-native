// using Alpha.Pesagem.Api.Models;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;

// namespace Alpha.Pesagem.Api.Data.ModelConfigurations
// {
//   public abstract class EntidadeTenantConfiguration<E> : IEntityTypeConfiguration<E> where E : EntidadeTenant
//   {
//     public virtual void Configure(EntityTypeBuilder<E> builder)
//     {
//       builder.HasOne(q => q.Fazenda)
//       .WithMany()
//       .HasForeignKey(q => q.FazendaId)
//       // .IsRequired()
//       .OnDelete(DeleteBehavior.Restrict);
//     }
//   }
// }