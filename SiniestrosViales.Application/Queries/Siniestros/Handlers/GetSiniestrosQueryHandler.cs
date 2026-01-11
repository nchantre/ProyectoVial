using AutoMapper;
using MediatR;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Application.Queries.Siniestros;
using SiniestrosViales.Domain.Interfaces;

namespace SiniestrosViales.Application.Queries.Siniestros.Handlers;

public class GetSiniestrosQueryHandler : IRequestHandler<GetSiniestrosQuery, PagedResult<SiniestroDto>>
{
    private readonly ISiniestroRepository _repository;
    private readonly IMapper _mapper;

    public GetSiniestrosQueryHandler(ISiniestroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<SiniestroDto>> Handle(GetSiniestrosQuery request, CancellationToken cancellationToken)
    {
        var siniestros = await _repository.GetWithFiltersAsync(
            request.DepartamentoId,
            request.FechaInicio,
            request.FechaFin,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var totalCount = await _repository.CountAsync(
            request.DepartamentoId,
            request.FechaInicio,
            request.FechaFin,
            cancellationToken);

        var siniestrosDto = _mapper.Map<List<SiniestroDto>>(siniestros);

        return new PagedResult<SiniestroDto>
        {
            Data = siniestrosDto,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
