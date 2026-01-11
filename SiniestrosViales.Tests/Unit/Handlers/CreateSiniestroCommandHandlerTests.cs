using FluentAssertions;
using Moq;
using SiniestrosViales.Application.Commands.Siniestros;
using SiniestrosViales.Application.Commands.Siniestros.Handlers;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.Interfaces;
using SiniestrosViales.Domain.ValueObjects;

namespace SiniestrosViales.Tests.Unit.Handlers;

public class CreateSiniestroCommandHandlerTests
{
    private readonly Mock<ISiniestroRepository> _repositoryMock;
    private readonly CreateSiniestroCommandHandler _handler;

    public CreateSiniestroCommandHandlerTests()
    {
        _repositoryMock = new Mock<ISiniestroRepository>();
        _handler = new CreateSiniestroCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ConDatosValidos_DeberiaCrearSiniestroYRetornarId()
    {
        // Arrange
        var siniestroId = Guid.NewGuid();
        var fechaHora = DateTime.UtcNow.AddHours(-1);
        
        var command = new CreateSiniestroCommand
        {
            Siniestro = new CreateSiniestroDto
            {
                FechaHora = fechaHora,
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

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Siniestro>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(siniestroId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(siniestroId);
        _repositoryMock.Verify(
            r => r.AddAsync(
                It.Is<Siniestro>(s => 
                    s.FechaHora == fechaHora &&
                    s.DepartamentoId == 1 &&
                    s.CiudadId == 1 &&
                    s.TipoSiniestroId == 1 &&
                    s.NumeroVictimas == 2 &&
                    s.Descripcion == "Colisión frontal" &&
                    s.Vehiculos.Count == 1
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_ConMultiplesVehiculos_DeberiaAgregarTodosLosVehiculos()
    {
        // Arrange
        var siniestroId = Guid.NewGuid();
        
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
                        Tipo = "Automóvil",
                        Placa = "ABC123",
                        Marca = "Toyota",
                        Modelo = "Corolla"
                    },
                    new VehiculoInvolucradoDto
                    {
                        Tipo = "Motocicleta",
                        Placa = "XYZ789",
                        Marca = "Honda",
                        Modelo = "CBR"
                    }
                }
            }
        };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Siniestro>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(siniestroId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(siniestroId);
        _repositoryMock.Verify(
            r => r.AddAsync(
                It.Is<Siniestro>(s => s.Vehiculos.Count == 2),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_ConDescripcionNula_DeberiaCrearSiniestroSinDescripcion()
    {
        // Arrange
        var siniestroId = Guid.NewGuid();
        
        var command = new CreateSiniestroCommand
        {
            Siniestro = new CreateSiniestroDto
            {
                FechaHora = DateTime.UtcNow.AddHours(-1),
                DepartamentoId = 1,
                CiudadId = 1,
                TipoSiniestroId = 1,
                NumeroVictimas = 0,
                Descripcion = null,
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

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Siniestro>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(siniestroId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(siniestroId);
        _repositoryMock.Verify(
            r => r.AddAsync(
                It.Is<Siniestro>(s => s.Descripcion == null),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}
