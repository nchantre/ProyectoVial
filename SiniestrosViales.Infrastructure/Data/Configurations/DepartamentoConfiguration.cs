using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiniestrosViales.Domain.Entities;

namespace SiniestrosViales.Infrastructure.Data.Configurations;

public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
{
    public void Configure(EntityTypeBuilder<Departamento> builder)
    {
        builder.ToTable("Departamentos");
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(d => d.Nombre)
            .HasColumnName("Nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(d => d.Nombre)
            .IsUnique()
            .HasDatabaseName("UQ_Departamentos_Nombre");

        builder.Property(d => d.CodigoDANE)
            .HasColumnName("CodigoDANE")
            .HasMaxLength(10);

        builder.Property(d => d.Activo)
            .HasColumnName("Activo")
            .IsRequired();

        builder.Property(d => d.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .IsRequired();

        // Ignorar FechaModificacion porque la tabla no tiene esta columna
        builder.Ignore(d => d.FechaModificacion);
    }
}
