using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.ValueObjects;

namespace SiniestrosViales.Infrastructure.Data.Configurations;

public class VehiculoInvolucradoConfiguration : IEntityTypeConfiguration<VehiculoInvolucrado>
{
    public void Configure(EntityTypeBuilder<VehiculoInvolucrado> builder)
    {
        builder.ToTable("VehiculosInvolucrados");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property<Guid>("SiniestroId")
            .HasColumnName("SiniestroId")
            .IsRequired();

        // Configurar la relación con Siniestro
        builder.HasOne<Siniestro>()
            .WithMany(s => s.Vehiculos)
            .HasForeignKey("SiniestroId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(v => v.Tipo)
            .HasColumnName("Tipo")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(v => v.Placa)
            .HasColumnName("Placa")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(v => v.Marca)
            .HasColumnName("Marca")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.Modelo)
            .HasColumnName("Modelo")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .IsRequired();

        // Ignorar FechaModificacion porque la tabla no tiene esta columna
        builder.Ignore(v => v.FechaModificacion);

        // Índice para Foreign Key
        builder.HasIndex("SiniestroId")
            .HasDatabaseName("IX_VehiculosInvolucrados_SiniestroId");
    }
}
