# Documentación de la Solución - API REST de Siniestros Viales

**Proyecto**: API REST para Gestión de Siniestros Viales  
**Tecnología**: .NET 8  
**Estado**: ~90% Completado

---

## 1. Modelo de Dominio

### 1.1 Estructura

El dominio está organizado en:

- **Entidades**: Siniestro (agregado raíz), TipoSiniestro, Departamento, Ciudad
- **Value Objects**: VehiculoInvolucrado
- **Interfaces**: ISiniestroRepository, ICatalogoRepository

### 1.2 Entidad Principal: Siniestro

**Propiedades**:
- Id (Guid), FechaHora, DepartamentoId, CiudadId, TipoSiniestroId
- NumeroVictimas, Descripcion, Vehiculos (colección)

**Reglas de Negocio**:
- Fecha no puede ser futura
- Número de víctimas >= 0
- Debe tener al menos un vehículo involucrado
- IDs de lookup deben existir en sus tablas

**Métodos de Dominio**:
- `AgregarVehiculo()`, `ActualizarDescripcion()`, `ActualizarNumeroVictimas()`

### 1.3 Entidades de Lookup

- **TipoSiniestro**: Catálogo de tipos (Colisión, Atropello, etc.)
- **Departamento**: Catálogo de departamentos de Colombia
- **Ciudad**: Catálogo de ciudades, relacionadas con departamentos

### 1.4 Value Object: VehiculoInvolucrado

Representa un vehículo involucrado con: Tipo, Placa, Marca, Modelo. Todos los campos son requeridos.

### 1.5 Diagrama de Relaciones

```
TipoSiniestro (1) ──→ (N) Siniestro (1) ──→ (N) VehiculoInvolucrado
                            │
                            ├──→ (N:1) Ciudad (N:1) ──→ Departamento
                            └──→ (N:1) Departamento
```

---

## 2. Architecture Decision Records (ADRs)

### ADR-001: Clean Architecture

**Decisión**: Implementar Clean Architecture con 4 capas (Domain, Application, Infrastructure, API).

**Razón**: Separación clara de responsabilidades, facilita pruebas, cumple con criterios de evaluación.

**Flujo de Dependencias**:
```
API → Application → Domain
     ↓
Infrastructure → Domain
```

**Consecuencias**: Mayor complejidad inicial, pero código más mantenible y testeable.

---

### ADR-002: CQRS con MediatR

**Decisión**: Separar operaciones de lectura (Queries) y escritura (Commands) usando MediatR.

**Razón**: Cumple con requisito de CQRS, desacopla controllers de lógica de negocio, facilita extensión.

**Estructura**:
- Commands: CreateSiniestroCommand
- Queries: GetSiniestrosQuery, GetSiniestroByIdQuery
- Handlers separados para cada operación

**Consecuencias**: Más archivos, pero código más organizado y fácil de mantener.

---

### ADR-003: Entity Framework Core Database First

**Decisión**: Usar EF Core 8.0 con enfoque Database First (sin migraciones).

**Razón**: Control total sobre esquema de BD, scripts SQL versionables, optimizaciones específicas de SQL Server.

**Implementación**:
- Base de datos creada manualmente con scripts SQL
- EF Core se conecta a BD existente
- Configuraciones mediante Fluent API

**Consecuencias**: Más configuración manual, pero mayor control sobre la BD.

---

### ADR-004: Repository Pattern

**Decisión**: Implementar Repository Pattern con interfaces en Domain e implementaciones en Infrastructure.

**Razón**: Abstrae acceso a datos, facilita pruebas unitarias, cumple con Dependency Inversion Principle.

**Estructura**:
- Interfaces: ISiniestroRepository, ICatalogoRepository (en Domain)
- Implementaciones: SiniestroRepository, CatalogoRepository (en Infrastructure)

**Consecuencias**: Capa adicional de abstracción, pero código más testeable y desacoplado.

---

## 3. Estado del Desarrollo

### 3.1 Completitud General: ~90%

**Fases Completadas (100%)**:
- ✅ Fase 1: Configuración y Dominio
- ✅ Fase 2: Aplicación - CQRS
- ✅ Fase 3: Infraestructura
- ✅ Fase 4: API - Endpoints
- ✅ Fase 5: Pruebas Esenciales (44 pruebas unitarias)
- ✅ Fase 8: Documentación

**Fases Parciales (80%)**:
- ⚠️ Fase 6: Pruebas y Validación (pruebas de integración pendientes)
- ⚠️ Fase 7: Optimización (logging avanzado opcional)

**Fase Pendiente**:
- ⚠️ Fase 9: Entrega Final (revisión final)

### 3.2 Funcionalidades Implementadas

✅ **Completamente Funcionales**:
- Registro de siniestros (POST /api/siniestros)
- Consulta con filtros: departamento, fechas, combinados (GET /api/siniestros)
- Paginación
- Consulta por ID (GET /api/siniestros/{id})
- Endpoints de catálogos (departamentos, ciudades, tipos)
- Validaciones con FluentValidation
- Manejo de errores con ProblemDetails
- 44 pruebas unitarias pasando

---

## 4. Partes que Tomaron Más Tiempo

### 4.1 Configuración de Entity Framework Core

**Tiempo**: ~4 horas (estimado: 2 horas)

**Razones**:
- Configuración de Fluent API para todas las entidades
- Ajuste de relaciones y Foreign Keys
- Resolución de problemas con propiedades de navegación al insertar
- Configuración de Value Objects (VehiculoInvolucrado)
- Ajuste de propiedades ignoradas (FechaModificacion en lookup tables)

### 4.2 Implementación de Pruebas Unitarias

**Tiempo**: ~4 horas (estimado: 2 horas)

**Razones**:
- Configuración de in-memory database para pruebas de Infrastructure
- Creación de datos de prueba (seed) para lookup tables
- Ajuste de pruebas de repositorio para reflejar proceso de dos pasos
- Configuración de AutoMapper en pruebas

### 4.3 Resolución de Problemas de Base de Datos

**Tiempo**: ~3 horas (estimado: 1 hora)

**Razones**:
- Migración de esquema antiguo (columnas string) a nuevo (Foreign Keys)
- Resolución de errores de Foreign Keys al insertar
- Ajuste de configuraciones EF Core para ignorar propiedades no mapeadas
- Depuración de problemas con propiedades de navegación null

### 4.4 Ajuste de CatalogosController a Arquitectura CQRS

**Tiempo**: ~1.5 horas (estimado: 30 minutos)

**Razones**:
- Refactorización de controller que accedía directamente a DbContext
- Creación de queries y handlers para catálogos
- Creación de ICatalogoRepository y su implementación

---

## 5. Construcción del Faltante

### 5.1 Pruebas de Integración

**Estado**: Creadas pero requieren configuración adicional

**Ubicación**: `SiniestrosViales.Tests/Integration/API/SiniestrosControllerTests.cs`

**Lo que falta**:
- Configuración completa de WebApplicationFactory
- Setup de base de datos en memoria para cada test
- Seed de datos de prueba más robusto
- Descomentar y ajustar los tests existentes

**Cómo completarlo**:
1. Configurar WebApplicationFactory con override de servicios
2. Usar base de datos en memoria única por test
3. Crear método helper para seed de datos
4. Implementar tests para todos los endpoints
5. Agregar tests de casos de error

**Tiempo estimado**: 2-3 horas

### 5.2 Logging Avanzado (Opcional)

**Estado**: ILogger básico disponible, Serilog pendiente

**Lo que falta**:
- Instalación de Serilog y sinks
- Configuración de Serilog en Program.cs
- Configuración de niveles de log por ambiente

**Cómo completarlo**:
1. Instalar paquetes: Serilog, Serilog.AspNetCore, Serilog.Sinks.Console, Serilog.Sinks.File
2. Configurar Serilog en Program.cs
3. Agregar configuración en appsettings.json

**Tiempo estimado**: 1-2 horas

**Nota**: Este elemento es opcional. El ILogger básico de .NET es suficiente para el proyecto.

---

## 6. Resumen

### 6.1 Objetivos Cumplidos

✅ Todos los requisitos funcionales implementados  
✅ Todos los requisitos técnicos cumplidos (Clean Architecture, DDD, CQRS, SOLID)  
✅ 44 pruebas unitarias implementadas (cobertura >50%)  
✅ Manejo robusto de errores  
✅ Documentación completa

### 6.2 Tiempo de Desarrollo

**Total utilizado**: ~26 horas (vs 48 horas estimadas inicialmente)

**Distribución**:
- Fases 1-5 (Core): ~21 horas
- Fases 6-7 (Refinamiento): ~4 horas
- Fase 8 (Documentación): ~1 hora

### 6.3 Elementos Pendientes

- Pruebas de integración (2-3 horas)
- Logging avanzado - opcional (1-2 horas)
- Revisión final y preparación para entrega

---

**Fin del Documento**
