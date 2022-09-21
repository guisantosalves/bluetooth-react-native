using System;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data.ModelConfigurations;
using Alpha.Pesagem.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Data
{
  public class AlphaDbContext : DbContext
  {
    public DbSet<Fazenda> Fazendas { get; set; }
    public HttpContext HttpContext { get; set; }

    private Fazenda _tenant;

    public Fazenda Tenant
    {
      get
      {
        this._tenant = this._tenant ?? new Fazenda { Id = Guid.Empty };
        return this._tenant;
      }
      set { _tenant = value; }
    }

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

      // Preenchendo tenant
      var entriesTenant = ChangeTracker.Entries()
          .Where(e => e.Entity is EntidadeTenant && (e.State == EntityState.Added || e.State == EntityState.Modified));

      foreach (var entityEntry in entriesTenant)
      {
        if ((this.Tenant == null) || ((EntidadeTenant)entityEntry.Entity).FazendaId != Guid.Empty)
        {
          continue;
        }

        ((EntidadeTenant)entityEntry.Entity).FazendaId = this.Tenant.Id;
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
      modelBuilder.ApplyConfiguration(new FornecedorModelConfiguration());
      modelBuilder.ApplyConfiguration(new PesoModelConfiguration());
      modelBuilder.ApplyConfiguration(new LogModelConfiguration());

      // Criação de índices
      
      modelBuilder.Entity<Fazenda>().HasIndex(em => em.Id);
      modelBuilder.Entity<Fornecedor>().HasIndex(em => em.FazendaId);
      modelBuilder.Entity<Log>().HasIndex(em => em.FazendaId);
      modelBuilder.Entity<Peso>().HasIndex(em => em.FazendaId);

      // Filtros por tenant. São aplicados globalmente no context.
      modelBuilder.Entity<Fazenda>().HasQueryFilter(e => e.Id == this.Tenant.Id);
      modelBuilder.Entity<Fornecedor>().HasQueryFilter(e => e.FazendaId == this.Tenant.Id);
      modelBuilder.Entity<Log>().HasQueryFilter(e => e.FazendaId == this.Tenant.Id);
      modelBuilder.Entity<Peso>().HasQueryFilter(e => e.FazendaId == this.Tenant.Id);
    }
  }
}
