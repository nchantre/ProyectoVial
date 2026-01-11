using AutoMapper;
using MediatR;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Application.Queries.Catalogos;
using SiniestrosViales.Domain.Interfaces;

namespace SiniestrosViales.Application.Queries.Catalogos.Handlers;

public class GetCiudadesQueryHandler : IRequestHandler<GetCiudadesQuery, List<CiudadDto>>
{
    private readonly ICatalogoRepository _repository;
    private readonly IMapper _mapper;

    public GetCiudadesQueryHandler(ICatalogoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CiudadDto>> Handle(GetCiudadesQuery request, CancellationToken cancellationToken)
    {
        var ciudades = await _repository.GetCiudadesActivasAsync(request.DepartamentoId, cancellationToken);
        return _mapper.Map<List<CiudadDto>>(ciudades);
    }
}
