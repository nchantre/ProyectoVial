using SiniestrosViales.Domain.Common;

namespace SiniestrosViales.Domain.ValueObjects;

public class VehiculoInvolucrado : Entity
{
    public string Tipo { get; private set; } = string.Empty;
    public string Placa { get; private set; } = string.Empty;
    public string Marca { get; private set; } = string.Empty;
    public string Modelo { get; private set; } = string.Empty;

    private VehiculoInvolucrado() { }

    public VehiculoInvolucrado(string tipo, string placa, string marca, string modelo)
    {
        if (string.IsNullOrWhiteSpace(tipo))
            throw new ArgumentException("El tipo de vehículo es requerido", nameof(tipo));
        
        if (string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("La placa es requerida", nameof(placa));
        
        if (string.IsNullOrWhiteSpace(marca))
            throw new ArgumentException("La marca es requerida", nameof(marca));
        
        if (string.IsNullOrWhiteSpace(modelo))
            throw new ArgumentException("El modelo es requerido", nameof(modelo));

        Tipo = tipo;
        Placa = placa;
        Marca = marca;
        Modelo = modelo;
    }
}
