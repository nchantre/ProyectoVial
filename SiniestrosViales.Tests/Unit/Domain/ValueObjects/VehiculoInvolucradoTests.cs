using FluentAssertions;
using SiniestrosViales.Domain.ValueObjects;

namespace SiniestrosViales.Tests.Unit.Domain.ValueObjects;

public class VehiculoInvolucradoTests
{
    [Fact]
    public void Constructor_ConDatosValidos_DeberiaCrearVehiculo()
    {
        // Arrange
        var tipo = "Automóvil";
        var placa = "ABC123";
        var marca = "Toyota";
        var modelo = "Corolla";

        // Act
        var vehiculo = new VehiculoInvolucrado(tipo, placa, marca, modelo);

        // Assert
        vehiculo.Should().NotBeNull();
        vehiculo.Tipo.Should().Be(tipo);
        vehiculo.Placa.Should().Be(placa);
        vehiculo.Marca.Should().Be(marca);
        vehiculo.Modelo.Should().Be(modelo);
        vehiculo.Id.Should().NotBeEmpty();
        vehiculo.FechaCreacion.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Constructor_ConTipoVacio_DeberiaLanzarArgumentException()
    {
        // Arrange
        var tipo = "";
        var placa = "ABC123";
        var marca = "Toyota";
        var modelo = "Corolla";

        // Act & Assert
        var act = () => new VehiculoInvolucrado(tipo, placa, marca, modelo);

        act.Should().Throw<ArgumentException>()
            .WithMessage("El tipo de vehículo es requerido*")
            .And.ParamName.Should().Be("tipo");
    }

    [Fact]
    public void Constructor_ConTipoNull_DeberiaLanzarArgumentException()
    {
        // Arrange
        string? tipo = null;
        var placa = "ABC123";
        var marca = "Toyota";
        var modelo = "Corolla";

        // Act & Assert
        var act = () => new VehiculoInvolucrado(tipo!, placa, marca, modelo);

        act.Should().Throw<ArgumentException>()
            .WithMessage("El tipo de vehículo es requerido*")
            .And.ParamName.Should().Be("tipo");
    }

    [Fact]
    public void Constructor_ConTipoSoloEspacios_DeberiaLanzarArgumentException()
    {
        // Arrange
        var tipo = "   ";
        var placa = "ABC123";
        var marca = "Toyota";
        var modelo = "Corolla";

        // Act & Assert
        var act = () => new VehiculoInvolucrado(tipo, placa, marca, modelo);

        act.Should().Throw<ArgumentException>()
            .WithMessage("El tipo de vehículo es requerido*")
            .And.ParamName.Should().Be("tipo");
    }

    [Fact]
    public void Constructor_ConPlacaVacia_DeberiaLanzarArgumentException()
    {
        // Arrange
        var tipo = "Automóvil";
        var placa = "";
        var marca = "Toyota";
        var modelo = "Corolla";

        // Act & Assert
        var act = () => new VehiculoInvolucrado(tipo, placa, marca, modelo);

        act.Should().Throw<ArgumentException>()
            .WithMessage("La placa es requerida*")
            .And.ParamName.Should().Be("placa");
    }

    [Fact]
    public void Constructor_ConMarcaVacia_DeberiaLanzarArgumentException()
    {
        // Arrange
        var tipo = "Automóvil";
        var placa = "ABC123";
        var marca = "";
        var modelo = "Corolla";

        // Act & Assert
        var act = () => new VehiculoInvolucrado(tipo, placa, marca, modelo);

        act.Should().Throw<ArgumentException>()
            .WithMessage("La marca es requerida*")
            .And.ParamName.Should().Be("marca");
    }

    [Fact]
    public void Constructor_ConModeloVacio_DeberiaLanzarArgumentException()
    {
        // Arrange
        var tipo = "Automóvil";
        var placa = "ABC123";
        var marca = "Toyota";
        var modelo = "";

        // Act & Assert
        var act = () => new VehiculoInvolucrado(tipo, placa, marca, modelo);

        act.Should().Throw<ArgumentException>()
            .WithMessage("El modelo es requerido*")
            .And.ParamName.Should().Be("modelo");
    }

    [Fact]
    public void Constructor_ConDiferentesTiposDeVehiculos_DeberiaCrearCorrectamente()
    {
        // Arrange & Act
        var automovil = new VehiculoInvolucrado("Automóvil", "ABC123", "Toyota", "Corolla");
        var motocicleta = new VehiculoInvolucrado("Motocicleta", "XYZ789", "Honda", "CBR");
        var camion = new VehiculoInvolucrado("Camión", "DEF456", "Mercedes", "Actros");
        var bus = new VehiculoInvolucrado("Bus", "GHI789", "Scania", "K270");

        // Assert
        automovil.Tipo.Should().Be("Automóvil");
        motocicleta.Tipo.Should().Be("Motocicleta");
        camion.Tipo.Should().Be("Camión");
        bus.Tipo.Should().Be("Bus");
    }

    [Fact]
    public void Constructor_ConPlacasDiferentes_DeberiaCrearVehiculosDiferentes()
    {
        // Arrange & Act
        var vehiculo1 = new VehiculoInvolucrado("Automóvil", "ABC123", "Toyota", "Corolla");
        var vehiculo2 = new VehiculoInvolucrado("Automóvil", "XYZ789", "Toyota", "Corolla");

        // Assert
        vehiculo1.Placa.Should().Be("ABC123");
        vehiculo2.Placa.Should().Be("XYZ789");
        vehiculo1.Id.Should().NotBe(vehiculo2.Id);
    }
}
