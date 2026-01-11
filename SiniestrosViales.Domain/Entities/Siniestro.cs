using SiniestrosViales.Domain.Common;
using SiniestrosViales.Domain.ValueObjects;

namespace SiniestrosViales.Domain.Entities;

public class Siniestro : Entity
{
    public DateTime FechaHora { get; private set; }
    public int DepartamentoId { get; private set; }
    public Departamento Departamento { get; private set; } = null!;
    public int CiudadId { get; private set; }
    public Ciudad Ciudad { get; private set; } = null!;
    public int TipoSiniestroId { get; private set; }
    public TipoSiniestro TipoSiniestro { get; private set; } = null!;
    public ICollection<VehiculoInvolucrado> Vehiculos { get; private set; } = new List<VehiculoInvolucrado>();
    public int NumeroVictimas { get; private set; }
    public string? Descripcion { get; private set; }

    private Siniestro() { }

    public Siniestro(
        DateTime fechaHora,
        int departamentoId,
        int ciudadId,
        int tipoSiniestroId,
        int numeroVictimas,
        string? descripcion = null)
    {
        if (fechaHora > DateTime.UtcNow)
            throw new ArgumentException("La fecha y hora no puede ser futura", nameof(fechaHora));

        if (numeroVictimas < 0)
            throw new ArgumentException("El número de víctimas no puede ser negativo", nameof(numeroVictimas));

        FechaHora = fechaHora;
        DepartamentoId = departamentoId;
        CiudadId = ciudadId;
        TipoSiniestroId = tipoSiniestroId;
        NumeroVictimas = numeroVictimas;
        Descripcion = descripcion;
    }

    public void AgregarVehiculo(VehiculoInvolucrado vehiculo)
    {
        if (vehiculo == null)
            throw new ArgumentNullException(nameof(vehiculo));

        Vehiculos.Add(vehiculo);
    }

    public void ActualizarDescripcion(string? descripcion)
    {
        Descripcion = descripcion;
        FechaModificacion = DateTime.UtcNow;
    }

    public void ActualizarNumeroVictimas(int numeroVictimas)
    {
        if (numeroVictimas < 0)
            throw new ArgumentException("El número de víctimas no puede ser negativo", nameof(numeroVictimas));

        NumeroVictimas = numeroVictimas;
        FechaModificacion = DateTime.UtcNow;
    }
}
