using MediatR;
using SiniestrosViales.Application.DTOs;

namespace SiniestrosViales.Application.Queries.Siniestros;

public class GetSiniestroByIdQuery : IRequest<SiniestroDto?>
{
    public Guid Id { get; set; }
}
