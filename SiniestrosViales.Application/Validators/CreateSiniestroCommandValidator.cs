using FluentValidation;
using SiniestrosViales.Application.Commands.Siniestros;

namespace SiniestrosViales.Application.Validators;

public class CreateSiniestroCommandValidator : AbstractValidator<CreateSiniestroCommand>
{
    public CreateSiniestroCommandValidator()
    {
        RuleFor(x => x.Siniestro.FechaHora)
            .NotEmpty().WithMessage("La fecha y hora es requerida")
            .Must(fecha => fecha <= DateTime.UtcNow)
            .WithMessage("La fecha y hora no puede ser futura");

        RuleFor(x => x.Siniestro.DepartamentoId)
            .GreaterThan(0).WithMessage("El departamento es requerido");

        RuleFor(x => x.Siniestro.CiudadId)
            .GreaterThan(0).WithMessage("La ciudad es requerida");

        RuleFor(x => x.Siniestro.TipoSiniestroId)
            .GreaterThan(0).WithMessage("El tipo de siniestro es requerido");

        RuleFor(x => x.Siniestro.NumeroVictimas)
            .GreaterThanOrEqualTo(0).WithMessage("El número de víctimas debe ser mayor o igual a 0");

        RuleFor(x => x.Siniestro.Vehiculos)
            .NotEmpty().WithMessage("Al menos un vehículo debe estar involucrado")
            .Must(vehiculos => vehiculos.Count > 0)
            .WithMessage("Al menos un vehículo debe estar involucrado");

        RuleForEach(x => x.Siniestro.Vehiculos)
            .SetValidator(new VehiculoInvolucradoDtoValidator());
    }
}

public class VehiculoInvolucradoDtoValidator : AbstractValidator<SiniestrosViales.Application.DTOs.VehiculoInvolucradoDto>
{
    public VehiculoInvolucradoDtoValidator()
    {
        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("El tipo de vehículo es requerido")
            .MaximumLength(50).WithMessage("El tipo de vehículo no puede exceder 50 caracteres");

        RuleFor(x => x.Placa)
            .NotEmpty().WithMessage("La placa es requerida")
            .MaximumLength(20).WithMessage("La placa no puede exceder 20 caracteres");

        RuleFor(x => x.Marca)
            .NotEmpty().WithMessage("La marca es requerida")
            .MaximumLength(100).WithMessage("La marca no puede exceder 100 caracteres");

        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("El modelo es requerido")
            .MaximumLength(100).WithMessage("El modelo no puede exceder 100 caracteres");
    }
}
