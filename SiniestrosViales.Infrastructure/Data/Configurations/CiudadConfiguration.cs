using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiniestrosViales.Domain.Entities;

namespace SiniestrosViales.Infrastructure.Data.Configurations;

public class CiudadConfiguration : IEntityTypeConfiguration<Ciudad>
{
    public void Configure(EntityTypeBuilder<Ciudad> builder)
    {
        builder.ToTable("Ciudades");
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(c => c.Nombre)
            .HasColumnName("Nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.DepartamentoId)
            .HasColumnName("DepartamentoId")
            .IsRequired();

        builder.HasOne(c => c.Departamento)
            .WithMany(d => d.Ciudades)
            .HasForeignKey(c => c.DepartamentoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(c => new { c.DepartamentoId, c.Nombre })
            .IsUnique()
            .HasDatabaseName("UQ_Ciudades_Departamento_Nombre");

        builder.Property(c => c.CodigoDANE)
            .HasColumnName("CodigoDANE")
            .HasMaxLength(10);

        builder.Property(c => c.Activo)
            .HasColumnName("Activo")
            .IsRequired();

        builder.Property(c => c.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .IsRequired();

        // Ignorar FechaModificacion porque la tabla no tiene esta columna
        builder.Ignore(c => c.FechaModificacion);
    }
}
