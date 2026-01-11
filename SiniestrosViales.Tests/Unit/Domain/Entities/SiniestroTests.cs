using FluentAssertions;
using SiniestrosViales.Domain.Entities;
using SiniestrosViales.Domain.ValueObjects;

namespace SiniestrosViales.Tests.Unit.Domain.Entities;

public class SiniestroTests
{
    [Fact]
    public void Constructor_ConDatosValidos_DeberiaCrearSiniestro()
    {
        // Arrange
        var fechaHora = DateTime.UtcNow.AddHours(-1);
        var departamentoId = 1;
        var ciudadId = 1;
        var tipoSiniestroId = 1;
        var numeroVictimas = 2;
        var descripcion = "Colisión frontal";

        // Act
        var siniestro = new Siniestro(
            fechaHora,
            departamentoId,
            ciudadId,
            tipoSiniestroId,
            numeroVictimas,
            descripcion);

        // Assert
        siniestro.Should().NotBeNull();
        siniestro.FechaHora.Should().Be(fechaHora);
        siniestro.DepartamentoId.Should().Be(departamentoId);
        siniestro.CiudadId.Should().Be(ciudadId);
        siniestro.TipoSiniestroId.Should().Be(tipoSiniestroId);
        siniestro.NumeroVictimas.Should().Be(numeroVictimas);
        siniestro.Descripcion.Should().Be(descripcion);
        siniestro.Vehiculos.Should().BeEmpty();
        siniestro.Id.Should().NotBeEmpty();
        siniestro.FechaCreacion.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Constructor_ConFechaFutura_DeberiaLanzarArgumentException()
    {
        // Arrange
        var fechaFutura = DateTime.UtcNow.AddHours(1);

        // Act & Assert
        var act = () => new Siniestro(
            fechaFutura,
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        act.Should().Throw<ArgumentException>()
            .WithMessage("La fecha y hora no puede ser futura*")
            .And.ParamName.Should().Be("fechaHora");
    }

    [Fact]
    public void Constructor_ConNumeroVictimasNegativo_DeberiaLanzarArgumentException()
    {
        // Arrange
        var fechaHora = DateTime.UtcNow.AddHours(-1);

        // Act & Assert
        var act = () => new Siniestro(
            fechaHora,
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: -1);

        act.Should().Throw<ArgumentException>()
            .WithMessage("El número de víctimas no puede ser negativo*")
            .And.ParamName.Should().Be("numeroVictimas");
    }

    [Fact]
    public void Constructor_SinDescripcion_DeberiaCrearSiniestroConDescripcionNula()
    {
        // Arrange
        var fechaHora = DateTime.UtcNow.AddHours(-1);

        // Act
        var siniestro = new Siniestro(
            fechaHora,
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        // Assert
        siniestro.Descripcion.Should().BeNull();
    }

    [Fact]
    public void AgregarVehiculo_ConVehiculoValido_DeberiaAgregarVehiculo()
    {
        // Arrange
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        var vehiculo = new VehiculoInvolucrado("Automóvil", "ABC123", "Toyota", "Corolla");

        // Act
        siniestro.AgregarVehiculo(vehiculo);

        // Assert
        siniestro.Vehiculos.Should().HaveCount(1);
        siniestro.Vehiculos.Should().Contain(vehiculo);
    }

    [Fact]
    public void AgregarVehiculo_ConVehiculoNulo_DeberiaLanzarArgumentNullException()
    {
        // Arrange
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        // Act & Assert
        var act = () => siniestro.AgregarVehiculo(null!);

        act.Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("vehiculo");
    }

    [Fact]
    public void AgregarVehiculo_ConMultiplesVehiculos_DeberiaAgregarTodos()
    {
        // Arrange
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        var vehiculo1 = new VehiculoInvolucrado("Automóvil", "ABC123", "Toyota", "Corolla");
        var vehiculo2 = new VehiculoInvolucrado("Motocicleta", "XYZ789", "Honda", "CBR");

        // Act
        siniestro.AgregarVehiculo(vehiculo1);
        siniestro.AgregarVehiculo(vehiculo2);

        // Assert
        siniestro.Vehiculos.Should().HaveCount(2);
        siniestro.Vehiculos.Should().Contain(vehiculo1);
        siniestro.Vehiculos.Should().Contain(vehiculo2);
    }

    [Fact]
    public void ActualizarDescripcion_ConNuevaDescripcion_DeberiaActualizarDescripcion()
    {
        // Arrange
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0,
            descripcion: "Descripción original");

        var nuevaDescripcion = "Descripción actualizada";

        // Act
        siniestro.ActualizarDescripcion(nuevaDescripcion);

        // Assert
        siniestro.Descripcion.Should().Be(nuevaDescripcion);
        siniestro.FechaModificacion.Should().NotBeNull();
        siniestro.FechaModificacion.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ActualizarDescripcion_ConDescripcionNula_DeberiaEstablecerDescripcionNula()
    {
        // Arrange
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0,
            descripcion: "Descripción original");

        // Act
        siniestro.ActualizarDescripcion(null);

        // Assert
        siniestro.Descripcion.Should().BeNull();
        siniestro.FechaModificacion.Should().NotBeNull();
    }

    [Fact]
    public void ActualizarNumeroVictimas_ConNumeroValido_DeberiaActualizarNumeroVictimas()
    {
        // Arrange
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        var nuevoNumero = 5;

        // Act
        siniestro.ActualizarNumeroVictimas(nuevoNumero);

        // Assert
        siniestro.NumeroVictimas.Should().Be(nuevoNumero);
        siniestro.FechaModificacion.Should().NotBeNull();
    }

    [Fact]
    public void ActualizarNumeroVictimas_ConNumeroNegativo_DeberiaLanzarArgumentException()
    {
        // Arrange
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 0);

        // Act & Assert
        var act = () => siniestro.ActualizarNumeroVictimas(-1);

        act.Should().Throw<ArgumentException>()
            .WithMessage("El número de víctimas no puede ser negativo*")
            .And.ParamName.Should().Be("numeroVictimas");
    }

    [Fact]
    public void ActualizarNumeroVictimas_ConCero_DeberiaActualizarCorrectamente()
    {
        // Arrange
        var siniestro = new Siniestro(
            DateTime.UtcNow.AddHours(-1),
            departamentoId: 1,
            ciudadId: 1,
            tipoSiniestroId: 1,
            numeroVictimas: 5);

        // Act
        siniestro.ActualizarNumeroVictimas(0);

        // Assert
        siniestro.NumeroVictimas.Should().Be(0);
        siniestro.FechaModificacion.Should().NotBeNull();
    }
}
