using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data.ModelConfigurations;
using Alpha.Pesagem.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Pesagem.Api.Data
{
  public class AlphaDbContext : DbContext
  {
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public HttpContext HttpContext { get; set; }

    private Empresa _tenant;

    public Empresa Tenant
    {
      get
      {
        this._tenant = this._tenant ?? new Empresa { Id = Guid.Empty };
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
        if ((this.Tenant == null) || ((EntidadeTenant)entityEntry.Entity).EmpresaId != Guid.Empty)
        {
          continue;
        }

        ((EntidadeTenant)entityEntry.Entity).EmpresaId = this.Tenant.Id;
      }

      if (this.HttpContext != null)
      {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
          // Preenchendo tenant
          var entriesUserLogs = ChangeTracker.Entries()
              .Where(e => e.Entity is IUsuarioLog && (e.State == EntityState.Added || e.State == EntityState.Modified));

          foreach (var entityEntry in entriesUserLogs)
          {
            var userId = this.HttpContext.User.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Sid).Value;

            if (entityEntry.State == EntityState.Added)
            {
              ((IUsuarioLog)entityEntry.Entity).UsuarioCriacaoId = Guid.Parse(userId);
            }
            else if (entityEntry.State == EntityState.Modified)
            {
              ((IUsuarioLog)entityEntry.Entity).UsuarioAlteracaoId = Guid.Parse(userId);
              this.Entry((IUsuarioLog)entityEntry.Entity).Property(q => q.UsuarioCriacaoId).IsModified = false;
            }
          }
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

      modelBuilder.ApplyConfiguration(new EmpresaModelConfiguration());
      modelBuilder.ApplyConfiguration(new FornecedorModelConfiguration());
      modelBuilder.ApplyConfiguration(new PesoModelConfiguration());
      modelBuilder.ApplyConfiguration(new LogModelConfiguration());
      modelBuilder.ApplyConfiguration(new UsuarioModelConfiguration());

      // Criação de índices
      
      modelBuilder.Entity<Fornecedor>().HasIndex(em => em.EmpresaId);
      modelBuilder.Entity<Log>().HasIndex(em => em.EmpresaId);
      modelBuilder.Entity<Usuario>().HasIndex(em => em.EmpresaId);
      modelBuilder.Entity<Peso>().HasIndex(em => em.EmpresaId);

      // Filtros por tenant. São aplicados globalmente no context.
      modelBuilder.Entity<Fornecedor>().HasQueryFilter(e => e.EmpresaId == this.Tenant.Id);
      modelBuilder.Entity<Log>().HasQueryFilter(e => e.EmpresaId == this.Tenant.Id);
      modelBuilder.Entity<Usuario>().HasQueryFilter(e => e.EmpresaId == this.Tenant.Id);
    }
  }
}
