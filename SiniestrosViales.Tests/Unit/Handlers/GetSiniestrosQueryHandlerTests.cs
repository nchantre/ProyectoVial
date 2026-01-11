using AutoMapper;
using FluentAssertions;
using Moq;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Application.Mappings;
using SiniestrosViales.Application.Queries.Siniestros;
using SiniestrosViales.Application.Queries.Siniestros.Handlers;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.Interfaces;
using SiniestrosViales.Domain.ValueObjects;

namespace SiniestrosViales.Tests.Unit.Handlers;

public class GetSiniestrosQueryHandlerTests
{
    private readonly Mock<ISiniestroRepository> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly GetSiniestrosQueryHandler _handler;

    public GetSiniestrosQueryHandlerTests()
    {
        _repositoryMock = new Mock<ISiniestroRepository>();
        
        // Configurar AutoMapper
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = mapperConfig.CreateMapper();
        
        _handler = new GetSiniestrosQueryHandler(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_SinFiltros_DeberiaRetornarTodosLosSiniestrosPaginados()
    {
        // Arrange
        var siniestros = new List<Siniestro>
        {
            CrearSiniestroEjemplo(Guid.NewGuid(), DateTime.UtcNow.AddDays(-1)),
            CrearSiniestroEjemplo(Guid.NewGuid(), DateTime.UtcNow.AddDays(-2))
        };

        var query = new GetSiniestrosQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        _repositoryMock
            .Setup(r => r.GetWithFiltersAsync(null, null, null, 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(siniestros);

        _repositoryMock
            .Setup(r => r.CountAsync(null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
        result.TotalPages.Should().Be(1);
        result.HasPrevious.Should().BeFalse();
        result.HasNext.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ConFiltroDepartamento_DeberiaFiltrarPorDepartamento()
    {
        // Arrange
        var siniestros = new List<Siniestro>
        {
            CrearSiniestroEjemplo(Guid.NewGuid(), DateTime.UtcNow.AddDays(-1))
        };

        var query = new GetSiniestrosQuery
        {
            DepartamentoId = 1,
            PageNumber = 1,
            PageSize = 10
        };

        _repositoryMock
            .Setup(r => r.GetWithFiltersAsync(1, null, null, 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(siniestros);

        _repositoryMock
            .Setup(r => r.CountAsync(1, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
        
        _repositoryMock.Verify(
            r => r.GetWithFiltersAsync(1, null, null, 1, 10, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_ConFiltroFechas_DeberiaFiltrarPorRangoDeFechas()
    {
        // Arrange
        var fechaInicio = DateTime.UtcNow.AddDays(-7);
        var fechaFin = DateTime.UtcNow;
        var siniestros = new List<Siniestro>
        {
            CrearSiniestroEjemplo(Guid.NewGuid(), DateTime.UtcNow.AddDays(-3))
        };

        var query = new GetSiniestrosQuery
        {
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            PageNumber = 1,
            PageSize = 10
        };

        _repositoryMock
            .Setup(r => r.GetWithFiltersAsync(null, fechaInicio, fechaFin, 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(siniestros);

        _repositoryMock
            .Setup(r => r.CountAsync(null, fechaInicio, fechaFin, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(1);
        
        _repositoryMock.Verify(
            r => r.GetWithFiltersAsync(null, fechaInicio, fechaFin, 1, 10, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_ConPaginacion_DeberiaCalcularTotalPagesCorrectamente()
    {
        // Arrange
        var siniestros = new List<Siniestro>
        {
            CrearSiniestroEjemplo(Guid.NewGuid(), DateTime.UtcNow.AddDays(-1))
        };

        var query = new GetSiniestrosQuery
        {
            PageNumber = 1,
            PageSize = 5
        };

        _repositoryMock
            .Setup(r => r.GetWithFiltersAsync(null, null, null, 1, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(siniestros);

        _repositoryMock
            .Setup(r => r.CountAsync(null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(12); // Total de 12 registros

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(12);
        result.TotalPages.Should().Be(3); // 12 / 5 = 2.4, redondeado a 3
        result.HasNext.Should().BeTrue();
    }

    private Siniestro CrearSiniestroEjemplo(Guid id, DateTime fechaHora)
    {
        var siniestro = new Siniestro(
            fechaHora,
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0,
            descripcion: "Test");

        // Usar reflexión para establecer el ID (solo para pruebas)
        var idProperty = typeof(Siniestro).BaseType?.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        if (idProperty != null && idProperty.CanWrite)
        {
            idProperty.SetValue(siniestro, id);
        }
        
        var vehiculo = new VehiculoInvolucrado("Automóvil", "ABC123", "Toyota", "Corolla");
        siniestro.AgregarVehiculo(vehiculo);

        return siniestro;
    }
}
