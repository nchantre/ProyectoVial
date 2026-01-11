namespace SiniestrosViales.Application.DTOs;

public class CreateSiniestroDto
{
    public DateTime FechaHora { get; set; }
    public int DepartamentoId { get; set; }
    public int CiudadId { get; set; }
    public int TipoSiniestroId { get; set; }
    public int NumeroVictimas { get; set; }
    public string? Descripcion { get; set; }
    public List<VehiculoInvolucradoDto> Vehiculos { get; set; } = new();
}

public class VehiculoInvolucradoDto
{
    public string Tipo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
}
