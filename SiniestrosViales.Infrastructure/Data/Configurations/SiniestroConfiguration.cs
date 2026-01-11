using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiniestrosViales.Domain.Entities;

namespace SiniestrosViales.Infrastructure.Data.Configurations;

public class SiniestroConfiguration : IEntityTypeConfiguration<Siniestro>
{
    public void Configure(EntityTypeBuilder<Siniestro> builder)
    {
        builder.ToTable("Siniestros");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(s => s.FechaHora)
            .HasColumnName("FechaHora")
            .IsRequired();

        builder.Property(s => s.DepartamentoId)
            .HasColumnName("DepartamentoId")
            .IsRequired();

        // Configurar relaciones - IMPORTANTE: No usar las propiedades de navegación al insertar
        builder.HasOne(s => s.Departamento)
            .WithMany()
            .HasForeignKey(s => s.DepartamentoId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.Property(s => s.CiudadId)
            .HasColumnName("CiudadId")
            .IsRequired();

        builder.HasOne(s => s.Ciudad)
            .WithMany()
            .HasForeignKey(s => s.CiudadId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.Property(s => s.TipoSiniestroId)
            .HasColumnName("TipoSiniestroId")
            .IsRequired();

        builder.HasOne(s => s.TipoSiniestro)
            .WithMany()
            .HasForeignKey(s => s.TipoSiniestroId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.Property(s => s.NumeroVictimas)
            .HasColumnName("NumeroVictimas")
            .IsRequired();

        builder.Property(s => s.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(int.MaxValue);

        builder.Property(s => s.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .IsRequired();

        builder.Property(s => s.FechaModificacion)
            .HasColumnName("FechaModificacion");

        // Relación con VehiculosInvolucrados
        builder.HasMany(s => s.Vehiculos)
            .WithOne()
            .HasForeignKey("SiniestroId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Índices
        builder.HasIndex(s => s.DepartamentoId)
            .HasDatabaseName("IX_Siniestros_DepartamentoId");

        builder.HasIndex(s => s.CiudadId)
            .HasDatabaseName("IX_Siniestros_CiudadId");

        builder.HasIndex(s => s.TipoSiniestroId)
            .HasDatabaseName("IX_Siniestros_TipoSiniestroId");

        builder.HasIndex(s => s.FechaHora)
            .HasDatabaseName("IX_Siniestros_FechaHora");

        builder.HasIndex(s => new { s.DepartamentoId, s.FechaHora })
            .HasDatabaseName("IX_Siniestros_DepartamentoId_FechaHora");
    }
}
