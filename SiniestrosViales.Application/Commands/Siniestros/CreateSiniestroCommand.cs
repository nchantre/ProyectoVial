using MediatR;
using SiniestrosViales.Application.DTOs;

namespace SiniestrosViales.Application.Commands.Siniestros;

public class CreateSiniestroCommand : IRequest<Guid>
{
    public CreateSiniestroDto Siniestro { get; set; } = null!;
}
