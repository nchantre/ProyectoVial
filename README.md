# API REST de Siniestros Viales

API REST desarrollada en .NET 8 para la gestión de siniestros viales, implementando Clean Architecture, Domain-Driven Design (DDD), CQRS y principios SOLID.

## 🏗️ Arquitectura

El proyecto sigue **Clean Architecture** con las siguientes capas:

- **Domain**: Entidades de dominio, Value Objects e interfaces de repositorio
- **Application**: Casos de uso (Commands/Queries con CQRS), DTOs, validaciones
- **Infrastructure**: Implementación de repositorios, Entity Framework Core, configuraciones
- **API**: Controladores REST, middleware, configuración de servicios

## 🚀 Requisitos Previos

- .NET 8 SDK
- SQL Server (Local o remoto)
- SQL Server Management Studio (opcional, para ejecutar scripts)

## 📦 Instalación

1. Clonar el repositorio
2. Restaurar paquetes NuGet:
   ```bash
   dotnet restore
   ```
3. Configurar la cadena de conexión en `SiniestrosViales.API/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=TU_SERVIDOR;Database=SiniestrosViales;Integrated Security=true;TrustServerCertificate=true;Encrypt=Optional;"
     }
   }
   ```

## 🗄️ Base de Datos

### Crear la Base de Datos

Ejecuta los scripts SQL en orden (en SQL Server Management Studio):

1. `Scripts/01_CreateDatabase.sql` - Crea la base de datos
2. `Scripts/02_CreateTables.sql` - Crea las tablas (lookup tables y Siniestros)
3. `Scripts/03_CreateIndexes.sql` - Crea los índices
4. `Scripts/04_InsertSampleData.sql` - (Opcional) Inserta datos de ejemplo

**Nota**: La base de datos se crea manualmente. No se usan migraciones de EF Core.

## ▶️ Ejecución

```bash
cd SiniestrosViales.API
dotnet run
```

La API estará disponible en:
- HTTP: `http://localhost:5184`
- Swagger UI: `http://localhost:5184/swagger`

## 📡 Endpoints

### Siniestros

- **POST** `/api/siniestros` - Crear un nuevo siniestro
- **GET** `/api/siniestros` - Listar siniestros (con filtros opcionales: `departamentoId`, `fechaInicio`, `fechaFin`, `pageNumber`, `pageSize`)
- **GET** `/api/siniestros/{id}` - Obtener siniestro por ID

### Catálogos

- **GET** `/api/catalogos/departamentos` - Listar departamentos
- **GET** `/api/catalogos/ciudades` - Listar ciudades
- **GET** `/api/catalogos/ciudades?departamentoId={id}` - Listar ciudades por departamento
- **GET** `/api/catalogos/tipos-siniestro` - Listar tipos de siniestro

## 🛠️ Tecnologías

- **.NET 8**
- **Entity Framework Core 8.0** (Database First - sin migraciones)
- **MediatR** (CQRS)
- **FluentValidation**
- **AutoMapper**
- **Swagger/OpenAPI**

## 📋 Estructura del Proyecto

```
SiniestrosViales/
├── SiniestrosViales.Domain/          # Capa de Dominio
├── SiniestrosViales.Application/      # Capa de Aplicación (CQRS)
├── SiniestrosViales.Infrastructure/   # Capa de Infraestructura
├── SiniestrosViales.API/              # Capa de Presentación
├── SiniestrosViales.Tests/            # Pruebas
└── Scripts/                            # Scripts SQL
```

## 📚 Documentación Adicional

- `PLAN_PROYECTO.md` - Plan completo del proyecto con fases de implementación
- `Scripts/ESTRUCTURA_TABLAS.md` - Estructura de la base de datos
- `Scripts/DECISION_DISENO_TABLAS.md` - Decisiones de diseño de la base de datos
- `Scripts/CONFIGURACION_EF_CORE.md` - Configuración de Entity Framework Core

## 🧪 Pruebas

El proyecto incluye **44 pruebas unitarias** distribuidas en:

- **Application Layer**: 15 pruebas (Handlers + Validators)
- **Domain Layer**: 19 pruebas (Entidades + Value Objects)
- **Infrastructure Layer**: 10 pruebas (Repositorio)

### Ejecutar Pruebas

```bash
# Todas las pruebas
dotnet test

# Solo pruebas unitarias
dotnet test --filter "FullyQualifiedName~Unit"

# Con cobertura (requiere coverlet)
dotnet test /p:CollectCoverage=true
```

## ✅ Estado del Proyecto

- ✅ **Fase 1**: Configuración y Dominio - Completada
- ✅ **Fase 2**: Aplicación (CQRS) - Completada
- ✅ **Fase 3**: Infraestructura - Completada
- ✅ **Fase 4**: API Endpoints - Completada
- ✅ **Fase 5**: Pruebas Esenciales - Completada (44 pruebas)



## 🏛️ Arquitectura

### Clean Architecture

El proyecto sigue **Clean Architecture** con separación clara de responsabilidades:

```
┌─────────────────────────────────────┐
│         API Layer                   │  ← Controllers, Middleware
│  (SiniestrosViales.API)            │
└──────────────┬──────────────────────┘
               │
               ↓
┌─────────────────────────────────────┐
│      Application Layer              │  ← Commands, Queries, DTOs
│  (SiniestrosViales.Application)    │     Validators, Mappings
└──────────────┬──────────────────────┘
               │
               ↓
┌─────────────────────────────────────┐
│        Domain Layer                 │  ← Entities, Value Objects
│  (SiniestrosViales.Domain)         │     Interfaces (Repositories)
└───────────────────────────────────────┘
               ↑
               │
┌──────────────┴──────────────────────┐
│     Infrastructure Layer             │  ← EF Core, Repositories
│  (SiniestrosViales.Infrastructure)  │     DbContext, Configurations
└──────────────────────────────────────┘
```

### Principios Aplicados

- **SOLID**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- **CQRS**: Separación de Commands (escritura) y Queries (lectura) usando MediatR
- **DDD**: Entidades ricas, Value Objects, Agregados, Repositorios
- **Repository Pattern**: Abstracción de acceso a datos
- **Mediator Pattern**: Desacoplamiento mediante MediatR

## 🔧 Desarrollo

### Estructura Detallada

```
SiniestrosViales/
├── SiniestrosViales.Domain/
│   ├── Common/              # Clases base (Entity, LookupEntity)
│   ├── Entities/            # Entidades de dominio
│   ├── ValueObjects/        # Objetos de valor
│   └── Interfaces/          # Contratos (ISiniestroRepository, ICatalogoRepository)
│
├── SiniestrosViales.Application/
│   ├── Commands/            # Comandos CQRS (CreateSiniestroCommand)
│   ├── Queries/             # Consultas CQRS (GetSiniestrosQuery, GetSiniestroByIdQuery)
│   ├── DTOs/                # Data Transfer Objects
│   ├── Mappings/            # AutoMapper profiles
│   └── Validators/          # FluentValidation validators
│
├── SiniestrosViales.Infrastructure/
│   ├── Data/
│   │   ├── DbContext/       # SiniestrosVialesDbContext
│   │   └── Configurations/ # Configuraciones Fluent API
│   └── Repositories/        # Implementaciones de repositorios
│
├── SiniestrosViales.API/
│   ├── Controllers/         # Controladores REST
│   ├── Middleware/          # GlobalExceptionHandlerMiddleware
│   └── Program.cs           # Configuración de servicios
│
└── SiniestrosViales.Tests/
    ├── Unit/                # Pruebas unitarias
    └── Integration/         # Pruebas de integración
```

## 📖 Ejemplos de Uso

### Crear un Siniestro

```bash
POST /api/siniestros
Content-Type: application/json

{
  "fechaHora": "2024-01-15T10:30:00",
  "departamentoId": 1,
  "ciudadId": 1,
  "tipoSiniestroId": 1,
  "numeroVictimas": 2,
  "descripcion": "Colisión frontal en intersección",
  "vehiculos": [
    {
      "tipo": "Automóvil",
      "placa": "ABC123",
      "marca": "Toyota",
      "modelo": "Corolla"
    }
  ]
}
```

### Consultar Siniestros con Filtros

```bash
GET /api/siniestros?departamentoId=1&fechaInicio=2024-01-01&fechaFin=2024-01-31&pageNumber=1&pageSize=10
```

## 🐛 Manejo de Errores

La API utiliza `ProblemDetails` (RFC 7807) para respuestas de error estructuradas:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred",
  "status": 400,
  "errors": {
    "fechaHora": ["La fecha y hora es requerida"]
  }
}
```

## 📚 Documentación Adicional

- `DOCUMENTACION_ENTREGA.md` - Documentación completa de la solución (modelo de dominio, ADRs, estado del desarrollo)
- `docs/ADR-001.md` - Clean Architecture
- `docs/ADR-002.md` - CQRS con MediatR
- `docs/ADR-003.md` - Entity Framework Core Database First
- `docs/ADR-004.md` - Repository Pattern
- `docs/MODELO_DOMINIO.md` - Documentación del modelo de dominio



