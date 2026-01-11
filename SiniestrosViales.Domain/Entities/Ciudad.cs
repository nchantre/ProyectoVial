using SiniestrosViales.Domain.Common;

namespace SiniestrosViales.Domain.Entities;

public class Ciudad : LookupEntity
{
    public string Nombre { get; private set; } = string.Empty;
    public int DepartamentoId { get; private set; }
    public Departamento Departamento { get; private set; } = null!;
    public string? CodigoDANE { get; private set; }
    public bool Activo { get; private set; } = true;

    private Ciudad() { }

    public Ciudad(string nombre, int departamentoId, string? codigoDANE = null)
    {
        Nombre = nombre;
        DepartamentoId = departamentoId;
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
