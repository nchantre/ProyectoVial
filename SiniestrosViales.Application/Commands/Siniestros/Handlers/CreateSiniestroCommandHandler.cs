using MediatR;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.Interfaces;
using SiniestrosViales.Domain.ValueObjects;
using SiniestrosViales.Application.Commands.Siniestros;

namespace SiniestrosViales.Application.Commands.Siniestros.Handlers;

public class CreateSiniestroCommandHandler : IRequestHandler<CreateSiniestroCommand, Guid>
{
    private readonly ISiniestroRepository _repository;

    public CreateSiniestroCommandHandler(ISiniestroRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateSiniestroCommand request, CancellationToken cancellationToken)
    {
        var siniestro = new Siniestro(
            request.Siniestro.FechaHora,
            request.Siniestro.DepartamentoId,
            request.Siniestro.CiudadId,
            request.Siniestro.TipoSiniestroId,
            request.Siniestro.NumeroVictimas,
            request.Siniestro.Descripcion);

        foreach (var vehiculoDto in request.Siniestro.Vehiculos)
        {
            var vehiculo = new VehiculoInvolucrado(
                vehiculoDto.Tipo,
                vehiculoDto.Placa,
                vehiculoDto.Marca,
                vehiculoDto.Modelo);
            
            siniestro.AgregarVehiculo(vehiculo);
        }

        return await _repository.AddAsync(siniestro, cancellationToken);
    }
}
