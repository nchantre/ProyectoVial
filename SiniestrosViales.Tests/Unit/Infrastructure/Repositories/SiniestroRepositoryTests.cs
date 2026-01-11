using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.ValueObjects;
using SiniestrosViales.Infrastructure.Data.DbContext;
using SiniestrosViales.Infrastructure.Repositories;

namespace SiniestrosViales.Tests.Unit.Infrastructure.Repositories;

public class SiniestroRepositoryTests : IDisposable
{
    private readonly SiniestrosVialesDbContext _context;
    private readonly SiniestroRepository _repository;

    public SiniestroRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<SiniestrosVialesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new SiniestrosVialesDbContext(options);
        _repository = new SiniestroRepository(_context);

        // Configurar datos de prueba (lookup tables)
        SeedLookupData();
    }

    [Fact]
    public async Task AddAsync_ConSiniestroValido_DeberiaAgregarYRetornarId()
    {
        // Arrange
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 2,
            descripcion: "Colisión frontal");

        var vehiculo = new VehiculoInvolucrado("Automóvil", "ABC123", "Toyota", "Corolla");
        siniestro.AgregarVehiculo(vehiculo);

        // Act
        var result = await _repository.AddAsync(siniestro);

        // Assert
        result.Should().NotBeEmpty();

        var siniestroGuardado = await _context.Siniestros
            .FirstOrDefaultAsync(s => s.Id == result);

        siniestroGuardado.Should().NotBeNull();
        siniestroGuardado!.NumeroVictimas.Should().Be(2);
        siniestroGuardado.Descripcion.Should().Be("Colisión frontal");

        // Verificar vehículos por separado
        var vehiculosGuardados = await _context.VehiculosInvolucrados
            .Where(v => EF.Property<Guid>(v, "SiniestroId") == result)
            .ToListAsync();
        
        vehiculosGuardados.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_ConIdExistente_DeberiaRetornarSiniestro()
    {
        // Arrange
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        var vehiculo = new VehiculoInvolucrado("Automóvil", "ABC123", "Toyota", "Corolla");
        siniestro.AgregarVehiculo(vehiculo);

        var id = await _repository.AddAsync(siniestro);

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.DepartamentoId.Should().Be(1);
        // Verificar vehículos a través de la relación
        var vehiculosCount = await _context.VehiculosInvolucrados
            .Where(v => EF.Property<Guid>(v, "SiniestroId") == id)
            .CountAsync();
        vehiculosCount.Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_ConIdInexistente_DeberiaRetornarNull()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(idInexistente);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetWithFiltersAsync_SinFiltros_DeberiaRetornarTodosLosSiniestros()
    {
        // Arrange
        await CrearSiniestrosDePrueba();

        // Act
        var result = await _repository.GetWithFiltersAsync();

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetWithFiltersAsync_ConFiltroDepartamento_DeberiaFiltrarPorDepartamento()
    {
        // Arrange
        await CrearSiniestrosDePrueba();

        // Act
        var result = await _repository.GetWithFiltersAsync(departamentoId: 1);

        // Assert
        result.Should().HaveCount(2); // Solo los siniestros del departamento 1
        result.All(s => s.DepartamentoId == 1).Should().BeTrue();
    }

    [Fact]
    public async Task GetWithFiltersAsync_ConFiltroFechas_DeberiaFiltrarPorRango()
    {
        // Arrange
        await CrearSiniestrosDePrueba();
        var fechaInicio = DateTime.UtcNow.AddDays(-3);
        var fechaFin = DateTime.UtcNow;

        // Act
        var result = await _repository.GetWithFiltersAsync(
            fechaInicio: fechaInicio,
            fechaFin: fechaFin);

        // Assert
        result.Should().NotBeEmpty();
        result.All(s => s.FechaHora >= fechaInicio && s.FechaHora <= fechaFin).Should().BeTrue();
    }

    [Fact]
    public async Task GetWithFiltersAsync_ConPaginacion_DeberiaRetornarPaginaCorrecta()
    {
        // Arrange
        await CrearSiniestrosDePrueba();

        // Act
        var pagina1 = await _repository.GetWithFiltersAsync(pageNumber: 1, pageSize: 2);
        var pagina2 = await _repository.GetWithFiltersAsync(pageNumber: 2, pageSize: 2);

        // Assert
        pagina1.Should().HaveCount(2);
        pagina2.Should().HaveCount(1);
    }

    [Fact]
    public async Task CountAsync_SinFiltros_DeberiaRetornarTotalCorrecto()
    {
        // Arrange
        await CrearSiniestrosDePrueba();

        // Act
        var result = await _repository.CountAsync();

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public async Task CountAsync_ConFiltroDepartamento_DeberiaContarSoloFiltrados()
    {
        // Arrange
        await CrearSiniestrosDePrueba();

        // Act
        var result = await _repository.CountAsync(departamentoId: 1);

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public async Task CountAsync_ConFiltroFechas_DeberiaContarSoloFiltrados()
    {
        // Arrange
        await CrearSiniestrosDePrueba();
        var fechaInicio = DateTime.UtcNow.AddDays(-3);
        var fechaFin = DateTime.UtcNow;

        // Act
        var result = await _repository.CountAsync(fechaInicio: fechaInicio, fechaFin: fechaFin);

        // Assert
        result.Should().BeGreaterThan(0);
        result.Should().BeLessThanOrEqualTo(3);
    }

    private async Task CrearSiniestrosDePrueba()
    {
        var siniestro1 = new Siniestro(
            DateTime.UtcNow.AddDays(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        var siniestro2 = new Siniestro(
            DateTime.UtcNow.AddDays(-2),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        var siniestro3 = new Siniestro(
            DateTime.UtcNow.AddDays(-10),
            departamentoId: 2,
            ciudadId: 2,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        await _repository.AddAsync(siniestro1);
        await _repository.AddAsync(siniestro2);
        await _repository.AddAsync(siniestro3);
    }

    private void SeedLookupData()
    {
        // Crear datos de lookup necesarios para las pruebas
        var departamento1 = new Departamento("Antioquia", "05");
        var departamento2 = new Departamento("Cundinamarca", "25");

        var ciudad1 = new Ciudad("Medellín", 1, "05001");
        var ciudad2 = new Ciudad("Bogotá", 2, "25001");

        var tipoSiniestro = new TipoSiniestro("Colisión", "Colisión entre vehículos");

        // Usar reflexión para establecer IDs en las entidades de lookup
        var dept1IdProperty = typeof(Departamento).BaseType?.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var dept2IdProperty = typeof(Departamento).BaseType?.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var ciudad1IdProperty = typeof(Ciudad).BaseType?.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var ciudad2IdProperty = typeof(Ciudad).BaseType?.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var tipoIdProperty = typeof(TipoSiniestro).BaseType?.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        if (dept1IdProperty != null && dept1IdProperty.CanWrite) dept1IdProperty.SetValue(departamento1, 1);
        if (dept2IdProperty != null && dept2IdProperty.CanWrite) dept2IdProperty.SetValue(departamento2, 2);
        if (ciudad1IdProperty != null && ciudad1IdProperty.CanWrite) ciudad1IdProperty.SetValue(ciudad1, 1);
        if (ciudad2IdProperty != null && ciudad2IdProperty.CanWrite) ciudad2IdProperty.SetValue(ciudad2, 2);
        if (tipoIdProperty != null && tipoIdProperty.CanWrite) tipoIdProperty.SetValue(tipoSiniestro, 1);

        _context.Departamentos.Add(departamento1);
        _context.Departamentos.Add(departamento2);
        _context.Ciudades.Add(ciudad1);
        _context.Ciudades.Add(ciudad2);
        _context.TiposSiniestro.Add(tipoSiniestro);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
