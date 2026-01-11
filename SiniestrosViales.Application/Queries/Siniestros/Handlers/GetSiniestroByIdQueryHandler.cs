using AutoMapper;
using MediatR;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Application.Queries.Siniestros;
using SiniestrosViales.Domain.Interfaces;

namespace SiniestrosViales.Application.Queries.Siniestros.Handlers;

public class GetSiniestroByIdQueryHandler : IRequestHandler<GetSiniestroByIdQuery, SiniestroDto?>
{
    private readonly ISiniestroRepository _repository;
    private readonly IMapper _mapper;

    public GetSiniestroByIdQueryHandler(ISiniestroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SiniestroDto?> Handle(GetSiniestroByIdQuery request, CancellationToken cancellationToken)
    {
        var siniestro = await _repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (siniestro == null)
            return null;

        return _mapper.Map<SiniestroDto>(siniestro);
    }
}
