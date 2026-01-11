# Modelo de Dominio - API REST de Siniestros Viales

## 📋 Visión General

El modelo de dominio representa el conocimiento y las reglas de negocio del dominio de siniestros viales. Está implementado siguiendo principios de **Domain-Driven Design (DDD)**.

## 🏗️ Estructura del Dominio

### Capas del Dominio

```
Domain/
├── Common/              # Clases base
│   ├── Entity.cs       # Entidad base con Id (GUID) y auditoría
│   └── LookupEntity.cs # Entidad base para catálogos (Id INT)
├── Entities/            # Entidades de dominio
│   ├── Siniestro.cs
│   ├── TipoSiniestro.cs
│   ├── Departamento.cs
│   └── Ciudad.cs
├── ValueObjects/        # Objetos de valor
│   └── VehiculoInvolucrado.cs
└── Interfaces/         # Contratos del dominio
    ├── ISiniestroRepository.cs
    └── ICatalogoRepository.cs
```

## 📦 Entidades

### 1. Siniestro (Agregado Raíz)

**Responsabilidad**: Representa un siniestro vial ocurrido en un lugar y momento específico.

**Propiedades**:
- `Id` (Guid): Identificador único
- `FechaHora` (DateTime): Fecha y hora del evento
- `DepartamentoId` (int): Foreign Key a Departamento
- `CiudadId` (int): Foreign Key a Ciudad
- `TipoSiniestroId` (int): Foreign Key a TipoSiniestro
- `NumeroVictimas` (int): Número de víctimas
- `Descripcion` (string?): Descripción opcional
- `Vehiculos` (ICollection<VehiculoInvolucrado>): Colección de vehículos involucrados
- `FechaCreacion` (DateTime): Fecha de creación
- `FechaModificacion` (DateTime?): Fecha de última modificación

**Reglas de Negocio**:
- La fecha y hora no puede ser futura
- El número de víctimas debe ser >= 0
- Debe tener al menos un vehículo involucrado
- Los IDs de lookup (DepartamentoId, CiudadId, TipoSiniestroId) deben existir en sus respectivas tablas

**Métodos de Dominio**:
- `AgregarVehiculo(VehiculoInvolucrado)`: Agrega un vehículo al siniestro
- `ActualizarDescripcion(string?)`: Actualiza la descripción
- `ActualizarNumeroVictimas(int)`: Actualiza el número de víctimas

**Relaciones**:
- Many-to-One con `Departamento`
- Many-to-One con `Ciudad`
- Many-to-One con `TipoSiniestro`
- One-to-Many con `VehiculoInvolucrado`

### 2. TipoSiniestro (Entidad de Lookup)

**Responsabilidad**: Representa un tipo de siniestro vial (Colisión, Atropello, etc.).

**Propiedades**:
- `Id` (int): Identificador único
- `Nombre` (string): Nombre del tipo
- `Descripcion` (string?): Descripción opcional
- `Activo` (bool): Indica si está activo
- `FechaCreacion` (DateTime): Fecha de creación

**Métodos de Dominio**:
- `Desactivar()`: Desactiva el tipo
- `Activar()`: Activa el tipo

**Uso**: Catálogo de tipos de siniestros. Se usa como Foreign Key en `Siniestro`.

### 3. Departamento (Entidad de Lookup)

**Responsabilidad**: Representa un departamento de Colombia.

**Propiedades**:
- `Id` (int): Identificador único
- `Nombre` (string): Nombre del departamento (UNIQUE)
- `CodigoDANE` (string?): Código DANE opcional
- `Activo` (bool): Indica si está activo
- `Ciudades` (ICollection<Ciudad>): Colección de ciudades
- `FechaCreacion` (DateTime): Fecha de creación

**Métodos de Dominio**:
- `Desactivar()`: Desactiva el departamento
- `Activar()`: Activa el departamento

**Uso**: Catálogo de departamentos. Se usa como Foreign Key en `Siniestro` y tiene relación con `Ciudad`.

### 4. Ciudad (Entidad de Lookup)

**Responsabilidad**: Representa una ciudad, perteneciente a un departamento.

**Propiedades**:
- `Id` (int): Identificador único
- `Nombre` (string): Nombre de la ciudad
- `DepartamentoId` (int): Foreign Key a Departamento
- `Departamento` (Departamento): Navegación a Departamento
- `CodigoDANE` (string?): Código DANE opcional
- `Activo` (bool): Indica si está activa
- `FechaCreacion` (DateTime): Fecha de creación

**Métodos de Dominio**:
- `Desactivar()`: Desactiva la ciudad
- `Activar()`: Activa la ciudad

**Reglas de Negocio**:
- Una ciudad debe pertenecer a un departamento
- El nombre de la ciudad debe ser único dentro del mismo departamento (UNIQUE constraint)

**Uso**: Catálogo de ciudades. Se usa como Foreign Key en `Siniestro`.

## 🎯 Value Objects

### VehiculoInvolucrado

**Responsabilidad**: Representa un vehículo involucrado en un siniestro.

**Propiedades**:
- `Id` (Guid): Identificador único (heredado de Entity)
- `Tipo` (string): Tipo de vehículo (Automóvil, Motocicleta, etc.)
- `Placa` (string): Placa del vehículo
- `Marca` (string): Marca del vehículo
- `Modelo` (string): Modelo del vehículo
- `FechaCreacion` (DateTime): Fecha de creación

**Reglas de Negocio**:
- Todos los campos son requeridos (no pueden ser null o vacíos)
- El tipo no puede exceder 50 caracteres
- La placa no puede exceder 20 caracteres
- La marca no puede exceder 100 caracteres
- El modelo no puede exceder 100 caracteres

**Inmutabilidad**: Una vez creado, un VehiculoInvolucrado no debe modificarse. Si se necesita cambiar, se crea uno nuevo.

**Uso**: Se almacena como entidad en la BD (por simplicidad), pero se trata como Value Object en el dominio.

## 🔗 Relaciones del Dominio

```
┌─────────────────┐
│ TipoSiniestro   │
│ (Lookup)        │
└────────┬────────┘
         │ 1:N
         │
┌────────▼────────┐      ┌──────────────────┐
│   Siniestro     │──────│ VehiculoInvolucrado │
│ (Aggregate Root)│ 1:N  │ (Value Object)   │
└────────┬────────┘      └──────────────────┘
         │
         │ N:1
         │
┌────────▼────────┐
│   Ciudad        │
│ (Lookup)       │
└────────┬────────┘
         │ N:1
         │
┌────────▼────────┐
│  Departamento  │
│ (Lookup)       │
└─────────────────┘
```

## 🎨 Clases Base

### Entity

Clase base para entidades principales del dominio.

```csharp
public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime FechaCreacion { get; protected set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; protected set; }
}
```

**Uso**: `Siniestro`, `VehiculoInvolucrado`

### LookupEntity

Clase base para entidades de catálogo (lookup tables).

```csharp
public abstract class LookupEntity
{
    public int Id { get; protected set; }
    public DateTime FechaCreacion { get; protected set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; protected set; }
}
```

**Uso**: `TipoSiniestro`, `Departamento`, `Ciudad`

## 🔐 Interfaces del Dominio

### ISiniestroRepository

Define los contratos para acceso a datos de siniestros.

```csharp
public interface ISiniestroRepository
{
    Task<Siniestro?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Siniestro>> GetWithFiltersAsync(
        int? departamentoId = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(
        int? departamentoId = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Siniestro siniestro, CancellationToken cancellationToken = default);
}
```

### ICatalogoRepository

Define los contratos para acceso a datos de catálogos.

```csharp
public interface ICatalogoRepository
{
    Task<IEnumerable<TipoSiniestro>> GetTiposSiniestroActivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Departamento>> GetDepartamentosActivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Ciudad>> GetCiudadesActivasAsync(int? departamentoId = null, CancellationToken cancellationToken = default);
}
```

## 📐 Principios DDD Aplicados

### 1. Entidades Ricas
- Las entidades contienen lógica de negocio (métodos de dominio)
- No son solo contenedores de datos (anémicas)

### 2. Value Objects
- `VehiculoInvolucrado` se trata como Value Object (inmutable, sin identidad propia)

### 3. Agregados
- `Siniestro` es el agregado raíz
- `VehiculoInvolucrado` pertenece al agregado `Siniestro`

### 4. Repositorios
- Interfaces definidas en Domain
- Implementaciones en Infrastructure
- Abstraen el acceso a datos

### 5. Inmutabilidad
- Las entidades usan setters privados
- Los cambios se hacen mediante métodos de dominio

## 🔄 Flujo de Datos

1. **Crear Siniestro**:
   - Controller recibe DTO
   - Command crea entidad Siniestro
   - Handler valida y guarda mediante Repository
   - Repository persiste en BD usando EF Core

2. **Consultar Siniestros**:
   - Controller recibe query parameters
   - Query se envía a Handler
   - Handler consulta Repository
   - Repository ejecuta consulta con filtros
   - Resultado se mapea a DTO y se retorna

## 📚 Referencias

- [Domain-Driven Design - Eric Evans](https://www.domainlanguage.com/ddd/)
- [DDD - Microsoft](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)
- [Value Objects - Martin Fowler](https://martinfowler.com/bliki/ValueObject.html)
