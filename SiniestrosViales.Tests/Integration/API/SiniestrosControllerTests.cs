using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using SiniestrosViales.Application.DTOs;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.ValueObjects;
using SiniestrosViales.Infrastructure.Data.DbContext;

namespace SiniestrosViales.Tests.Integration.API;

public class SiniestrosControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly SiniestrosVialesDbContext _context;

    public SiniestrosControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remover el DbContext real
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<SiniestrosVialesDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Agregar DbContext en memoria para pruebas
                services.AddDbContext<SiniestrosVialesDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid().ToString());
                });
            });
        });

        _client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<SiniestrosVialesDbContext>();
        SeedTestData();
    }

    [Fact]
    public async Task POST_Siniestros_ConDatosValidos_DeberiaRetornar201Created()
    {
        // Arrange
        var siniestroDto = new CreateSiniestroDto
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
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/siniestros", siniestroDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task POST_Siniestros_ConDatosInvalidos_DeberiaRetornar400BadRequest()
    {
        // Arrange
        var siniestroDto = new CreateSiniestroDto
        {
            FechaHora = DateTime.UtcNow.AddHours(1), // Fecha futura (inválida)
            DepartamentoId = 0, // Inválido
            CiudadId = 0, // Inválido
            TipoSiniestroId = 0, // Inválido
            NumeroVictimas = -1, // Inválido
            Vehiculos = new List<VehiculoInvolucradoDto>() // Lista vacía (inválido)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/siniestros", siniestroDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GET_Siniestros_SinFiltros_DeberiaRetornar200OK()
    {
        // Arrange
        await CrearSiniestroDePrueba();

        // Act
        var response = await _client.GetAsync("/api/siniestros");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PagedResult<SiniestroDto>>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GET_Siniestros_ConFiltroDepartamento_DeberiaFiltrarCorrectamente()
    {
        // Arrange
        await CrearSiniestrosDePrueba();

        // Act
        var response = await _client.GetAsync("/api/siniestros?departamentoId=1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PagedResult<SiniestroDto>>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        result.Should().NotBeNull();
        result!.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GET_Siniestros_ConPaginacion_DeberiaRetornarPaginaCorrecta()
    {
        // Arrange
        await CrearSiniestrosDePrueba();

        // Act
        var response = await _client.GetAsync("/api/siniestros?pageNumber=1&pageSize=2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PagedResult<SiniestroDto>>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        result.Should().NotBeNull();
        result!.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(2);
        result.Data.Should().HaveCountLessThanOrEqualTo(2);
    }

    [Fact]
    public async Task GET_Siniestros_ConIdExistente_DeberiaRetornar200OK()
    {
        // Arrange
        var siniestroId = await CrearSiniestroDePrueba();

        // Act
        var response = await _client.GetAsync($"/api/siniestros/{siniestroId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<SiniestroDto>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        result.Should().NotBeNull();
        result!.Id.Should().Be(siniestroId);
    }

    [Fact]
    public async Task GET_Siniestros_ConIdInexistente_DeberiaRetornar404NotFound()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/siniestros/{idInexistente}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<Guid> CrearSiniestroDePrueba()
    {
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        var vehiculo = new VehiculoInvolucrado("Automóvil", "ABC123", "Toyota", "Corolla");
        siniestro.AgregarVehiculo(vehiculo);

        _context.Siniestros.Add(siniestro);
        await _context.SaveChangesAsync();

        return siniestro.Id;
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

        _context.Siniestros.Add(siniestro1);
        _context.Siniestros.Add(siniestro2);
        await _context.SaveChangesAsync();
    }

    private void SeedTestData()
    {
        var departamento = new Departamento("Antioquia", "05");
        var ciudad = new Ciudad("Medellín", 1, "05001");
        var tipoSiniestro = new TipoSiniestro("Colisión", "Colisión entre vehículos");

        var deptIdProperty = typeof(Departamento).BaseType?.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var ciudadIdProperty = typeof(Ciudad).BaseType?.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var tipoIdProperty = typeof(TipoSiniestro).BaseType?.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        if (deptIdProperty != null && deptIdProperty.CanWrite) deptIdProperty.SetValue(departamento, 1);
        if (ciudadIdProperty != null && ciudadIdProperty.CanWrite) ciudadIdProperty.SetValue(ciudad, 1);
        if (tipoIdProperty != null && tipoIdProperty.CanWrite) tipoIdProperty.SetValue(tipoSiniestro, 1);

        _context.Departamentos.Add(departamento);
        _context.Ciudades.Add(ciudad);
        _context.TiposSiniestro.Add(tipoSiniestro);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context?.Dispose();
        _scope?.Dispose();
        _client?.Dispose();
    }
}

// Nota: Las pruebas de integración requieren que Program.cs esté en un namespace accesible
// Para simplificar, estas pruebas están comentadas. Se pueden habilitar cuando Program.cs
// esté en un namespace accesible o usando InternalsVisibleTo.
