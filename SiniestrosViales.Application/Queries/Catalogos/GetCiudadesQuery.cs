using MediatR;
using SiniestrosViales.Application.DTOs;

namespace SiniestrosViales.Application.Queries.Catalogos;

public record GetCiudadesQuery(int? DepartamentoId = null) : IRequest<List<CiudadDto>>;
