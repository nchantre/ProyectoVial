using AutoMapper;
using MediatR;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Application.Queries.Catalogos;
using SiniestrosViales.Domain.Interfaces;

namespace SiniestrosViales.Application.Queries.Catalogos.Handlers;

public class GetTiposSiniestroQueryHandler : IRequestHandler<GetTiposSiniestroQuery, List<TipoSiniestroDto>>
{
    private readonly ICatalogoRepository _repository;
    private readonly IMapper _mapper;

    public GetTiposSiniestroQueryHandler(ICatalogoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<TipoSiniestroDto>> Handle(GetTiposSiniestroQuery request, CancellationToken cancellationToken)
    {
        var tipos = await _repository.GetTiposSiniestroActivosAsync(cancellationToken);
        return _mapper.Map<List<TipoSiniestroDto>>(tipos);
    }
}
