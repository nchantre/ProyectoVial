namespace SiniestrosViales.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime FechaCreacion { get; protected set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; protected set; }

    protected void SetId(Guid id)
    {
        if (Id == Guid.Empty)
        {
            Id = id;
        }
    }
}
