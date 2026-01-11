-- =============================================
-- Script de Creación de Índices
-- Proyecto: API REST de Siniestros Viales
-- Optimización para consultas por departamento y fechas
-- =============================================

USE SiniestrosViales;
GO

-- =============================================
-- Índice para búsquedas por DepartamentoId (Foreign Key)
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
    AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Siniestros_DepartamentoId' AND object_id = OBJECT_ID('dbo.Siniestros'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Siniestros_DepartamentoId]
    ON [dbo].[Siniestros] ([DepartamentoId] ASC);
    PRINT 'Índice IX_Siniestros_DepartamentoId creado exitosamente.';
END
ELSE IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
BEGIN
    PRINT 'ERROR: La tabla Siniestros no existe. Ejecute primero el script 02_CreateTables.sql';
END
ELSE
BEGIN
    PRINT 'El índice IX_Siniestros_DepartamentoId ya existe.';
END
GO

-- =============================================
-- Índice para búsquedas por FechaHora
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
    AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Siniestros_FechaHora' AND object_id = OBJECT_ID('dbo.Siniestros'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Siniestros_FechaHora]
    ON [dbo].[Siniestros] ([FechaHora] ASC);
    PRINT 'Índice IX_Siniestros_FechaHora creado exitosamente.';
END
ELSE IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
BEGIN
    PRINT 'ERROR: La tabla Siniestros no existe. Ejecute primero el script 02_CreateTables.sql';
END
ELSE
BEGIN
    PRINT 'El índice IX_Siniestros_FechaHora ya existe.';
END
GO

-- =============================================
-- Índice compuesto para búsquedas combinadas (DepartamentoId + FechaHora)
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
    AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Siniestros_DepartamentoId_FechaHora' AND object_id = OBJECT_ID('dbo.Siniestros'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Siniestros_DepartamentoId_FechaHora]
    ON [dbo].[Siniestros] ([DepartamentoId] ASC, [FechaHora] ASC);
    PRINT 'Índice compuesto IX_Siniestros_DepartamentoId_FechaHora creado exitosamente.';
END
ELSE IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
BEGIN
    PRINT 'ERROR: La tabla Siniestros no existe. Ejecute primero el script 02_CreateTables.sql';
END
ELSE
BEGIN
    PRINT 'El índice IX_Siniestros_DepartamentoId_FechaHora ya existe.';
END
GO

-- =============================================
-- Índice para búsquedas por TipoSiniestroId (Foreign Key)
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
    AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Siniestros_TipoSiniestroId' AND object_id = OBJECT_ID('dbo.Siniestros'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Siniestros_TipoSiniestroId]
    ON [dbo].[Siniestros] ([TipoSiniestroId] ASC);
    PRINT 'Índice IX_Siniestros_TipoSiniestroId creado exitosamente.';
END
ELSE IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
BEGIN
    PRINT 'ERROR: La tabla Siniestros no existe. Ejecute primero el script 02_CreateTables.sql';
END
ELSE
BEGIN
    PRINT 'El índice IX_Siniestros_TipoSiniestroId ya existe.';
END
GO

-- =============================================
-- Índice para búsquedas por CiudadId (Foreign Key)
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
    AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Siniestros_CiudadId' AND object_id = OBJECT_ID('dbo.Siniestros'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Siniestros_CiudadId]
    ON [dbo].[Siniestros] ([CiudadId] ASC);
    PRINT 'Índice IX_Siniestros_CiudadId creado exitosamente.';
END
ELSE IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
BEGIN
    PRINT 'ERROR: La tabla Siniestros no existe. Ejecute primero el script 02_CreateTables.sql';
END
ELSE
BEGIN
    PRINT 'El índice IX_Siniestros_CiudadId ya existe.';
END
GO

-- =============================================
-- Índice para Foreign Key en VehiculosInvolucrados
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VehiculosInvolucrados]') AND type in (N'U'))
    AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_VehiculosInvolucrados_SiniestroId' AND object_id = OBJECT_ID('dbo.VehiculosInvolucrados'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_VehiculosInvolucrados_SiniestroId]
    ON [dbo].[VehiculosInvolucrados] ([SiniestroId] ASC);
    PRINT 'Índice IX_VehiculosInvolucrados_SiniestroId creado exitosamente.';
END
ELSE IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VehiculosInvolucrados]') AND type in (N'U'))
BEGIN
    PRINT 'ERROR: La tabla VehiculosInvolucrados no existe. Ejecute primero el script 02_CreateTables.sql';
END
ELSE
BEGIN
    PRINT 'El índice IX_VehiculosInvolucrados_SiniestroId ya existe.';
END
GO
