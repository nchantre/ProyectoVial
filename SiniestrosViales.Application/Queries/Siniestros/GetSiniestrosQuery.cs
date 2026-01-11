using MediatR;
using SiniestrosViales.Application.DTOs;

namespace SiniestrosViales.Application.Queries.Siniestros;

public class GetSiniestrosQuery : IRequest<PagedResult<SiniestroDto>>
{
    public int? DepartamentoId { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
