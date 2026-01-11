using Microsoft.EntityFrameworkCore;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.ValueObjects;
using SiniestrosViales.Infrastructure.Data.Configurations;

namespace SiniestrosViales.Infrastructure.Data.DbContext;

public class SiniestrosVialesDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public SiniestrosVialesDbContext(DbContextOptions<SiniestrosVialesDbContext> options)
        : base(options)
    {
    }

    public DbSet<TipoSiniestro> TiposSiniestro { get; set; }
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Ciudad> Ciudades { get; set; }
    public DbSet<Siniestro> Siniestros { get; set; }
    public DbSet<VehiculoInvolucrado> VehiculosInvolucrados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones
        modelBuilder.ApplyConfiguration(new TipoSiniestroConfiguration());
        modelBuilder.ApplyConfiguration(new DepartamentoConfiguration());
        modelBuilder.ApplyConfiguration(new CiudadConfiguration());
        modelBuilder.ApplyConfiguration(new SiniestroConfiguration());
        modelBuilder.ApplyConfiguration(new VehiculoInvolucradoConfiguration());
    }
}
