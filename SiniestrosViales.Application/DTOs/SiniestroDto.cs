namespace SiniestrosViales.Application.DTOs;

public class SiniestroDto
{
    public Guid Id { get; set; }
    public DateTime FechaHora { get; set; }
    public DepartamentoDto Departamento { get; set; } = null!;
    public CiudadDto Ciudad { get; set; } = null!;
    public TipoSiniestroDto TipoSiniestro { get; set; } = null!;
    public int NumeroVictimas { get; set; }
    public string? Descripcion { get; set; }
    public List<VehiculoInvolucradoDto> Vehiculos { get; set; } = new();
}

public class DepartamentoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? CodigoDANE { get; set; }
}

public class CiudadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? CodigoDANE { get; set; }
}

public class TipoSiniestroDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
