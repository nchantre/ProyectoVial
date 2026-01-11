using SiniestrosViales.Domain.Common;

namespace SiniestrosViales.Domain.Entities;

public class Departamento : LookupEntity
{
    public string Nombre { get; private set; } = string.Empty;
    public string? CodigoDANE { get; private set; }
    public bool Activo { get; private set; } = true;
    public ICollection<Ciudad> Ciudades { get; private set; } = new List<Ciudad>();

    private Departamento() { }

    public Departamento(string nombre, string? codigoDANE = null)
    {
        Nombre = nombre;
        CodigoDANE = codigoDANE;
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
