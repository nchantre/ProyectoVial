using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiniestrosViales.Domain.Entities;

namespace SiniestrosViales.Infrastructure.Data.Configurations;

public class TipoSiniestroConfiguration : IEntityTypeConfiguration<TipoSiniestro>
{
    public void Configure(EntityTypeBuilder<TipoSiniestro> builder)
    {
        builder.ToTable("TiposSiniestro");
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(t => t.Nombre)
            .HasColumnName("Nombre")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(200);

        builder.Property(t => t.Activo)
            .HasColumnName("Activo")
            .IsRequired();

        builder.Property(t => t.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .IsRequired();

        // Ignorar FechaModificacion porque la tabla no tiene esta columna
        builder.Ignore(t => t.FechaModificacion);
    }
}
