using FluentAssertions;
using FluentValidation.TestHelper;
using SiniestrosViales.Application.Commands.Siniestros;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Application.Validators;

namespace SiniestrosViales.Tests.Unit.Validators;

public class CreateSiniestroCommandValidatorTests
{
    private readonly CreateSiniestroCommandValidator _validator;

    public CreateSiniestroCommandValidatorTests()
    {
        _validator = new CreateSiniestroCommandValidator();
    }

    [Fact]
    public void Validar_ConDatosValidos_DeberiaSerValido()
    {
        // Arrange
        var command = new CreateSiniestroCommand
        {
            Siniestro = new CreateSiniestroDto
            {
                FechaHora = DateTime.UtcNow.AddHours(-1),
                DepartamentoId = 1,
                CiudadId = 1,
                TipoSiniestroId = 1,
                NumeroVictimas = 2,
                Descripcion = "Colisión frontal",
                Vehiculos = new List<VehiculoInvolucradoDto>
                {
                    new VehiculoInvolucradoDto
                    {
                        Tipo = "Automóvil",
                        Placa = "ABC123",
                        Marca = "Toyota",
                        Modelo = "Corolla"
                    }
                }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_ConFechaHoraFutura_DeberiaSerInvalido()
    {
        // Arrange
        var command = new CreateSiniestroCommand
        {
            Siniestro = new CreateSiniestroDto
            {
                FechaHora = DateTime.UtcNow.AddHours(1), // Fecha futura
                DepartamentoId = 1,
                CiudadId = 1,
                TipoSiniestroId = 1,
                NumeroVictimas = 0,
                Vehiculos = new List<VehiculoInvolucradoDto>
                {
                    new VehiculoInvolucradoDto
                    {
                        Tipo = "Automóvil",
                        Placa = "ABC123",
                        Marca = "Toyota",
                        Modelo = "Corolla"
                    }
                }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Siniestro.FechaHora)
            .WithErrorMessage("La fecha y hora no puede ser futura");
    }

    [Fact]
    public void Validar_ConDepartamentoIdCero_DeberiaSerInvalido()
    {
        // Arrange
        var command = new CreateSiniestroCommand
        {
            Siniestro = new CreateSiniestroDto
            {
                FechaHora = DateTime.UtcNow.AddHours(-1),
                DepartamentoId = 0, // Inválido
                CiudadId = 1,
                TipoSiniestroId = 1,
                NumeroVictimas = 0,
                Vehiculos = new List<VehiculoInvolucradoDto>
                {
                    new VehiculoInvolucradoDto
                    {
                        Tipo = "Automóvil",
                        Placa = "ABC123",
                        Marca = "Toyota",
                        Modelo = "Corolla"
                    }
                }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Siniestro.DepartamentoId)
            .WithErrorMessage("El departamento es requerido");
    }

    [Fact]
    public void Validar_ConNumeroVictimasNegativo_DeberiaSerInvalido()
    {
        // Arrange
        var command = new CreateSiniestroCommand
        {
            Siniestro = new CreateSiniestroDto
            {
                FechaHora = DateTime.UtcNow.AddHours(-1),
                DepartamentoId = 1,
                CiudadId = 1,
                TipoSiniestroId = 1,
                NumeroVictimas = -1, // Inválido
                Vehiculos = new List<VehiculoInvolucradoDto>
                {
                    new VehiculoInvolucradoDto
                    {
                        Tipo = "Automóvil",
                        Placa = "ABC123",
                        Marca = "Toyota",
                        Modelo = "Corolla"
                    }
                }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Siniestro.NumeroVictimas)
            .WithErrorMessage("El número de víctimas debe ser mayor o igual a 0");
    }

    [Fact]
    public void Validar_SinVehiculos_DeberiaSerInvalido()
    {
        // Arrange
        var command = new CreateSiniestroCommand
        {
            Siniestro = new CreateSiniestroDto
            {
                FechaHora = DateTime.UtcNow.AddHours(-1),
                DepartamentoId = 1,
                CiudadId = 1,
                TipoSiniestroId = 1,
                NumeroVictimas = 0,
                Vehiculos = new List<VehiculoInvolucradoDto>() // Lista vacía
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Siniestro.Vehiculos)
            .WithErrorMessage("Al menos un vehículo debe estar involucrado");
    }

    [Fact]
    public void Validar_ConVehiculoInvalido_DeberiaSerInvalido()
    {
        // Arrange
        var command = new CreateSiniestroCommand
        {
            Siniestro = new CreateSiniestroDto
            {
                FechaHora = DateTime.UtcNow.AddHours(-1),
                DepartamentoId = 1,
                CiudadId = 1,
                TipoSiniestroId = 1,
                NumeroVictimas = 0,
                Vehiculos = new List<VehiculoInvolucradoDto>
                {
                    new VehiculoInvolucradoDto
                    {
                        Tipo = "", // Inválido
                        Placa = "",
                        Marca = "",
                        Modelo = ""
                    }
                }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Siniestro.Vehiculos[0].Tipo");
        result.ShouldHaveValidationErrorFor("Siniestro.Vehiculos[0].Placa");
        result.ShouldHaveValidationErrorFor("Siniestro.Vehiculos[0].Marca");
        result.ShouldHaveValidationErrorFor("Siniestro.Vehiculos[0].Modelo");
    }

    [Fact]
    public void Validar_ConCiudadIdCero_DeberiaSerInvalido()
    {
        // Arrange
        var command = new CreateSiniestroCommand
        {
            Siniestro = new CreateSiniestroDto
            {
                FechaHora = DateTime.UtcNow.AddHours(-1),
                DepartamentoId = 1,
                CiudadId = 0, // Inválido
                TipoSiniestroId = 1,
                NumeroVictimas = 0,
                Vehiculos = new List<VehiculoInvolucradoDto>
                {
                    new VehiculoInvolucradoDto
                    {
                        Tipo = "Automóvil",
                        Placa = "ABC123",
                        Marca = "Toyota",
                        Modelo = "Corolla"
                    }
                }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Siniestro.CiudadId)
            .WithErrorMessage("La ciudad es requerida");
    }

    [Fact]
    public void Validar_ConTipoSiniestroIdCero_DeberiaSerInvalido()
    {
        // Arrange
        var command = new CreateSiniestroCommand
        {
            Siniestro = new CreateSiniestroDto
            {
                FechaHora = DateTime.UtcNow.AddHours(-1),
                DepartamentoId = 1,
                CiudadId = 1,
                TipoSiniestroId = 0, // Inválido
                NumeroVictimas = 0,
                Vehiculos = new List<VehiculoInvolucradoDto>
                {
                    new VehiculoInvolucradoDto
                    {
                        Tipo = "Automóvil",
                        Placa = "ABC123",
                        Marca = "Toyota",
                        Modelo = "Corolla"
                    }
                }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Siniestro.TipoSiniestroId)
            .WithErrorMessage("El tipo de siniestro es requerido");
    }
}
