# Scripts SQL para Base de Datos - Siniestros Viales

## 📋 Descripción

Este directorio contiene los scripts SQL necesarios para crear y configurar la base de datos SQL Server para el proyecto API REST de Siniestros Viales.

## 🗂️ Archivos

### 1. `01_CreateDatabase.sql`
Crea la base de datos `SiniestrosViales` si no existe.

**Ejecutar primero:** Este script debe ejecutarse primero para crear la base de datos.

### 2. `02_CreateTables.sql`
Crea las tablas principales y de lookup:
- **TiposSiniestro**: Catálogo de tipos de siniestros (Colisión, Atropello, etc.)
- **Departamentos**: Catálogo de departamentos de Colombia
- **Ciudades**: Catálogo de ciudades (relacionadas con departamentos)
- **Siniestros**: Almacena la información principal de los siniestros viales (con Foreign Keys)
- **VehiculosInvolucrados**: Almacena los vehículos involucrados en cada siniestro

**Ejecutar segundo:** Después de crear la base de datos.

**Nota:** Este script también inserta datos iniciales en las tablas de lookup (TiposSiniestro, Departamentos, Ciudades).

### 3. `03_CreateIndexes.sql`
Crea los índices necesarios para optimizar las consultas:
- Índice por Departamento
- Índice por FechaHora
- Índice compuesto (Departamento + FechaHora)
- Índice por TipoSiniestro
- Índice para Foreign Key

**Ejecutar tercero:** Después de crear las tablas.

### 4. `04_InsertSampleData.sql` (Opcional)
Inserta datos de prueba para desarrollo y testing.

**Ejecutar opcionalmente:** Solo si necesitas datos de prueba para probar la API.

## 🚀 Instrucciones de Ejecución

### Opción 1: SQL Server Management Studio (SSMS)

1. Abrir SQL Server Management Studio
2. Conectarse a tu instancia de SQL Server
3. Abrir cada archivo `.sql` en orden (01, 02, 03, 04)
4. Ejecutar cada script presionando `F5` o haciendo clic en "Execute"

### Opción 2: Azure Data Studio

1. Abrir Azure Data Studio
2. Conectarse a tu instancia de SQL Server
3. Abrir cada archivo `.sql` en orden
4. Ejecutar cada script

### Opción 3: Línea de Comandos (sqlcmd)

```bash
# Ejecutar todos los scripts en orden
sqlcmd -S localhost -d master -i Scripts/01_CreateDatabase.sql
sqlcmd -S localhost -d SiniestrosViales -i Scripts/02_CreateTables.sql
sqlcmd -S localhost -d SiniestrosViales -i Scripts/03_CreateIndexes.sql

# Opcional: Datos de prueba
sqlcmd -S localhost -d SiniestrosViales -i Scripts/04_InsertSampleData.sql
```

### Opción 4: PowerShell

```powershell
# Configurar variables
$serverName = "localhost"
$databaseName = "SiniestrosViales"

# Ejecutar scripts
Invoke-Sqlcmd -ServerInstance $serverName -InputFile "Scripts/01_CreateDatabase.sql"
Invoke-Sqlcmd -ServerInstance $serverName -Database $databaseName -InputFile "Scripts/02_CreateTables.sql"
Invoke-Sqlcmd -ServerInstance $serverName -Database $databaseName -InputFile "Scripts/03_CreateIndexes.sql"
Invoke-Sqlcmd -ServerInstance $serverName -Database $databaseName -InputFile "Scripts/04_InsertSampleData.sql"
```

## 📊 Estructura de la Base de Datos

### Tabla: TiposSiniestro (Lookup)
- `Id` (INT, PK, Identity): Identificador único
- `Nombre` (NVARCHAR(50)): Nombre del tipo
- `Descripcion` (NVARCHAR(200), NULL): Descripción opcional
- `Activo` (BIT): Indica si está activo

### Tabla: Departamentos (Lookup)
- `Id` (INT, PK, Identity): Identificador único
- `Nombre` (NVARCHAR(100), UNIQUE): Nombre del departamento
- `CodigoDANE` (NVARCHAR(10), NULL): Código DANE opcional
- `Activo` (BIT): Indica si está activo

### Tabla: Ciudades (Lookup)
- `Id` (INT, PK, Identity): Identificador único
- `Nombre` (NVARCHAR(100)): Nombre de la ciudad
- `DepartamentoId` (INT, FK): Referencia a Departamentos
- `CodigoDANE` (NVARCHAR(10), NULL): Código DANE opcional
- `Activo` (BIT): Indica si está activa

### Tabla: Siniestros
- `Id` (UNIQUEIDENTIFIER, PK): Identificador único del siniestro
- `FechaHora` (DATETIME2): Fecha y hora del evento
- `DepartamentoId` (INT, FK): Referencia a Departamentos
- `CiudadId` (INT, FK): Referencia a Ciudades
- `TipoSiniestroId` (INT, FK): Referencia a TiposSiniestro
- `NumeroVictimas` (INT): Número de víctimas
- `Descripcion` (NVARCHAR(MAX), NULL): Descripción opcional
- `FechaCreacion` (DATETIME2): Fecha de creación del registro
- `FechaModificacion` (DATETIME2, NULL): Fecha de última modificación

### Tabla: VehiculosInvolucrados
- `Id` (UNIQUEIDENTIFIER, PK): Identificador único del vehículo
- `SiniestroId` (UNIQUEIDENTIFIER, FK): Referencia al siniestro
- `Tipo` (NVARCHAR(50)): Tipo de vehículo (Automóvil, Motocicleta, Camión, etc.)
- `Placa` (NVARCHAR(20)): Placa del vehículo
- `Marca` (NVARCHAR(100)): Marca del vehículo
- `Modelo` (NVARCHAR(100)): Modelo del vehículo
- `FechaCreacion` (DATETIME2): Fecha de creación del registro

## 🔄 Scripts de Rollback (Opcional)

Si necesitas eliminar la base de datos y empezar de nuevo:

```sql
USE master;
GO
DROP DATABASE IF EXISTS SiniestrosViales;
GO
```

## ⚠️ Notas Importantes

1. **Idioma**: Los scripts están preparados para manejar caracteres especiales (NVARCHAR)
2. **Fechas**: Se usa `DATETIME2` para mayor precisión
3. **GUIDs**: Se usan `UNIQUEIDENTIFIER` para los IDs (puedes cambiar a INT IDENTITY si prefieres)
4. **Cascada**: La eliminación de un siniestro elimina automáticamente sus vehículos (ON DELETE CASCADE)
5. **Índices**: Los índices están optimizados para las consultas más frecuentes (por departamento y fecha)

## 🔧 Personalización

Si necesitas modificar la estructura:
- Cambiar el nombre de la base de datos: Editar `01_CreateDatabase.sql`
- Modificar campos: Editar `02_CreateTables.sql`
- Agregar más índices: Editar `03_CreateIndexes.sql`

## 📝 Próximos Pasos

Después de ejecutar los scripts:
1. Verificar que las tablas se crearon correctamente
2. Verificar que los índices se crearon correctamente
3. Configurar la cadena de conexión en `appsettings.json` del proyecto API
4. Probar la conexión desde la aplicación
