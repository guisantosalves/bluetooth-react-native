using System;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data.ModelConfigurations;
using Alpha.Pesagem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Data
{
  public class AlphaDbContext : DbContext
  {
    public DbSet<Fazenda> Fazendas { get; set; }

    public AlphaDbContext(DbContextOptions<AlphaDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.EnableSensitiveDataLogging(true);
      base.OnConfiguring(optionsBuilder);
    }

    public void SobrescreverSaveChanges()
    {
      // Preenchendo data automaticamente
      var entries = ChangeTracker
          .Entries()
          .Where(e => e.Entity is IDateLog && (e.State == EntityState.Added || e.State == EntityState.Modified));

      foreach (var entityEntry in entries)
      {
        if (entityEntry.State == EntityState.Added)
        {
          ((IDateLog)entityEntry.Entity).DataCriacao = DateTime.UtcNow;
        }
        else if (entityEntry.State == EntityState.Modified)
        {
          this.Entry((IDateLog)entityEntry.Entity).Property(q => q.DataCriacao).IsModified = false;
          ((IDateLog)entityEntry.Entity).DataAlteracao = DateTime.UtcNow;
        }
      }
    }

    public override int SaveChanges()
    {
      this.SobrescreverSaveChanges();
      return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
    {
      this.SobrescreverSaveChanges();

      return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.ApplyConfiguration(new FazendaModelConfiguration());
      modelBuilder.ApplyConfiguration(new RefreshTokenModelConfiguration());
      modelBuilder.ApplyConfiguration(new PesoModelConfiguration());
      modelBuilder.ApplyConfiguration(new LogModelConfiguration());

      // Criação de índices
      
      modelBuilder.Entity<Fazenda>().HasIndex(em => em.Id);
      modelBuilder.Entity<RefreshToken>().HasIndex(em => em.FazendaId);
      modelBuilder.Entity<Peso>().HasIndex(em => em.FazendaId);
      modelBuilder.Entity<Log>().HasIndex(em => em.FazendaId);
    }
  }
}
