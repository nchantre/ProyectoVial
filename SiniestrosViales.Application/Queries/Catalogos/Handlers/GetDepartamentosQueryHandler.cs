using AutoMapper;
using MediatR;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Application.Queries.Catalogos;
using SiniestrosViales.Domain.Interfaces;

namespace SiniestrosViales.Application.Queries.Catalogos.Handlers;

public class GetDepartamentosQueryHandler : IRequestHandler<GetDepartamentosQuery, List<DepartamentoDto>>
{
    private readonly ICatalogoRepository _repository;
    private readonly IMapper _mapper;

    public GetDepartamentosQueryHandler(ICatalogoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<DepartamentoDto>> Handle(GetDepartamentosQuery request, CancellationToken cancellationToken)
    {
        var departamentos = await _repository.GetDepartamentosActivosAsync(cancellationToken);
        return _mapper.Map<List<DepartamentoDto>>(departamentos);
    }
}
