-- =============================================
-- Script de Datos de Prueba (Opcional)
-- Proyecto: API REST de Siniestros Viales
-- NOTA: Este script asume que las tablas de lookup ya tienen datos
-- (se insertan automáticamente en 02_CreateTables.sql)
-- =============================================

USE SiniestrosViales;
GO

-- Limpiar datos existentes (solo para desarrollo)
-- DELETE FROM VehiculosInvolucrados;
-- DELETE FROM Siniestros;
-- GO

-- =============================================
-- Obtener IDs de las tablas de lookup
-- =============================================
DECLARE @DepartamentoCundinamarca INT = (SELECT Id FROM Departamentos WHERE Nombre = 'Cundinamarca');
DECLARE @DepartamentoAntioquia INT = (SELECT Id FROM Departamentos WHERE Nombre = 'Antioquia');
DECLARE @DepartamentoValle INT = (SELECT Id FROM Departamentos WHERE Nombre = 'Valle del Cauca');
DECLARE @DepartamentoAtlantico INT = (SELECT Id FROM Departamentos WHERE Nombre = 'Atlántico');

DECLARE @CiudadBogota INT = (SELECT Id FROM Ciudades WHERE Nombre = 'Bogotá');
DECLARE @CiudadMedellin INT = (SELECT Id FROM Ciudades WHERE Nombre = 'Medellín');
DECLARE @CiudadCali INT = (SELECT Id FROM Ciudades WHERE Nombre = 'Cali');
DECLARE @CiudadBarranquilla INT = (SELECT Id FROM Ciudades WHERE Nombre = 'Barranquilla');

DECLARE @TipoColision INT = (SELECT Id FROM TiposSiniestro WHERE Nombre = 'Colisión');
DECLARE @TipoAtropello INT = (SELECT Id FROM TiposSiniestro WHERE Nombre = 'Atropello');
DECLARE @TipoVolcamiento INT = (SELECT Id FROM TiposSiniestro WHERE Nombre = 'Volcamiento');

-- =============================================
-- Insertar Siniestros de Prueba
-- =============================================

-- Siniestro 1: Colisión en Bogotá
DECLARE @SiniestroId1 UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[Siniestros] ([Id], [FechaHora], [DepartamentoId], [CiudadId], [TipoSiniestroId], [NumeroVictimas], [Descripcion])
VALUES (@SiniestroId1, '2024-01-15T10:30:00', @DepartamentoCundinamarca, @CiudadBogota, @TipoColision, 2, 'Colisión frontal en intersección de la Calle 72 con Carrera 7');

INSERT INTO [dbo].[VehiculosInvolucrados] ([SiniestroId], [Tipo], [Placa], [Marca], [Modelo])
VALUES 
    (@SiniestroId1, 'Automóvil', 'ABC123', 'Toyota', 'Corolla'),
    (@SiniestroId1, 'Automóvil', 'XYZ789', 'Chevrolet', 'Spark');

-- Siniestro 2: Atropello en Medellín
DECLARE @SiniestroId2 UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[Siniestros] ([Id], [FechaHora], [DepartamentoId], [CiudadId], [TipoSiniestroId], [NumeroVictimas], [Descripcion])
VALUES (@SiniestroId2, '2024-01-20T14:15:00', @DepartamentoAntioquia, @CiudadMedellin, @TipoAtropello, 1, 'Atropello en la Avenida El Poblado');

INSERT INTO [dbo].[VehiculosInvolucrados] ([SiniestroId], [Tipo], [Placa], [Marca], [Modelo])
VALUES 
    (@SiniestroId2, 'Motocicleta', 'DEF456', 'Yamaha', 'MT-07');

-- Siniestro 3: Volcamiento en Cali
DECLARE @SiniestroId3 UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[Siniestros] ([Id], [FechaHora], [DepartamentoId], [CiudadId], [TipoSiniestroId], [NumeroVictimas], [Descripcion])
VALUES (@SiniestroId3, '2024-02-01T08:45:00', @DepartamentoValle, @CiudadCali, @TipoVolcamiento, 3, 'Volcamiento de camión en la Autopista Sur');

INSERT INTO [dbo].[VehiculosInvolucrados] ([SiniestroId], [Tipo], [Placa], [Marca], [Modelo])
VALUES 
    (@SiniestroId3, 'Camión', 'GHI789', 'Mercedes-Benz', 'Actros');

-- Siniestro 4: Colisión en Barranquilla
DECLARE @SiniestroId4 UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[Siniestros] ([Id], [FechaHora], [DepartamentoId], [CiudadId], [TipoSiniestroId], [NumeroVictimas], [Descripcion])
VALUES (@SiniestroId4, '2024-02-10T16:20:00', @DepartamentoAtlantico, @CiudadBarranquilla, @TipoColision, 0, 'Colisión lateral en la Vía 40');

INSERT INTO [dbo].[VehiculosInvolucrados] ([SiniestroId], [Tipo], [Placa], [Marca], [Modelo])
VALUES 
    (@SiniestroId4, 'Automóvil', 'JKL012', 'Ford', 'Focus'),
    (@SiniestroId4, 'Automóvil', 'MNO345', 'Renault', 'Logan');

PRINT 'Datos de prueba insertados exitosamente.';
GO
