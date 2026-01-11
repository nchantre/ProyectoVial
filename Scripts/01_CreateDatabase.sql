-- =============================================
-- Script de Creación de Base de Datos
-- Proyecto: API REST de Siniestros Viales
-- Base de Datos: SiniestrosViales
-- =============================================

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SiniestrosViales')
BEGIN
    CREATE DATABASE SiniestrosViales;
    PRINT 'Base de datos SiniestrosViales creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos SiniestrosViales ya existe.';
END
GO

-- Usar la base de datos
USE SiniestrosViales;
GO
