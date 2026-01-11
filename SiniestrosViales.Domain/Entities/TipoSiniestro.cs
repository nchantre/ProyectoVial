using SiniestrosViales.Domain.Common;

namespace SiniestrosViales.Domain.Entities;

public class TipoSiniestro : LookupEntity
{
    public string Nombre { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public bool Activo { get; private set; } = true;

    private TipoSiniestro() { }

    public TipoSiniestro(string nombre, string? descripcion = null)
    {
        Nombre = nombre;
        Descripcion = descripcion;
        Activo = true;
    }

    public void Desactivar()
    {
        Activo = false;
    }

    public void Activar()
    {
        Activo = true;
    }
}
