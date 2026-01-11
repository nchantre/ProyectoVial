namespace SiniestrosViales.Domain.Common;

public abstract class LookupEntity
{
    public int Id { get; protected set; }
    public DateTime FechaCreacion { get; protected set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; protected set; }
}
