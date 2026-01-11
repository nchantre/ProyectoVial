using MediatR;
using SiniestrosViales.Application.DTOs;

namespace SiniestrosViales.Application.Queries.Catalogos;

public record GetTiposSiniestroQuery : IRequest<List<TipoSiniestroDto>>;
