-- =============================================
-- Script de Creación de Tablas
-- Proyecto: API REST de Siniestros Viales
-- =============================================

USE SiniestrosViales;
GO

-- =============================================
-- Tabla: TiposSiniestro (Lookup Table)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TiposSiniestro]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[TiposSiniestro] (
        [Id] INT NOT NULL IDENTITY(1,1),
        [Nombre] NVARCHAR(50) NOT NULL,
        [Descripcion] NVARCHAR(200) NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_TiposSiniestro] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    -- Insertar valores iniciales
    INSERT INTO [dbo].[TiposSiniestro] ([Nombre], [Descripcion]) VALUES
        ('Colisión', 'Choque entre vehículos'),
        ('Atropello', 'Atropello a peatón'),
        ('Volcamiento', 'Volcamiento de vehículo'),
        ('Incendio', 'Incendio en vehículo'),
        ('Otro', 'Otro tipo de siniestro');
    
    PRINT 'Tabla TiposSiniestro creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla TiposSiniestro ya existe.';
END
GO

-- =============================================
-- Tabla: Departamentos (Lookup Table)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Departamentos]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Departamentos] (
        [Id] INT NOT NULL IDENTITY(1,1),
        [Nombre] NVARCHAR(100) NOT NULL,
        [CodigoDANE] NVARCHAR(10) NULL, -- Código DANE opcional
        [Activo] BIT NOT NULL DEFAULT 1,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Departamentos] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_Departamentos_Nombre] UNIQUE ([Nombre])
    );
    
    -- Insertar algunos departamentos comunes de Colombia
    INSERT INTO [dbo].[Departamentos] ([Nombre], [CodigoDANE]) VALUES
        ('Cundinamarca', '25'),
        ('Antioquia', '05'),
        ('Valle del Cauca', '76'),
        ('Atlántico', '08'),
        ('Santander', '68'),
        ('Bolívar', '13'),
        ('Córdoba', '23'),
        ('Nariño', '52'),
        ('Cauca', '19'),
        ('Tolima', '73');
    
    PRINT 'Tabla Departamentos creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla Departamentos ya existe.';
END
GO

-- =============================================
-- Tabla: Ciudades (Lookup Table)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ciudades]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Ciudades] (
        [Id] INT NOT NULL IDENTITY(1,1),
        [Nombre] NVARCHAR(100) NOT NULL,
        [DepartamentoId] INT NOT NULL,
        [CodigoDANE] NVARCHAR(10) NULL, -- Código DANE opcional
        [Activo] BIT NOT NULL DEFAULT 1,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Ciudades] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Ciudades_Departamentos] FOREIGN KEY ([DepartamentoId]) 
            REFERENCES [dbo].[Departamentos] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [UQ_Ciudades_Departamento_Nombre] UNIQUE ([DepartamentoId], [Nombre])
    );
    
    -- Insertar algunas ciudades comunes
    INSERT INTO [dbo].[Ciudades] ([Nombre], [DepartamentoId], [CodigoDANE]) VALUES
        ('Bogotá', 1, '11001'),
        ('Medellín', 2, '05001'),
        ('Cali', 3, '76001'),
        ('Barranquilla', 4, '08001'),
        ('Cartagena', 6, '13001'),
        ('Bucaramanga', 5, '68001'),
        ('Pereira', 3, '66001'),
        ('Santa Marta', 6, '47001'),
        ('Manizales', 2, '17001'),
        ('Ibagué', 10, '73001');
    
    PRINT 'Tabla Ciudades creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla Ciudades ya existe.';
END
GO

-- =============================================
-- Tabla: Siniestros
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Siniestros]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Siniestros] (
        [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [FechaHora] DATETIME2 NOT NULL,
        [DepartamentoId] INT NOT NULL,
        [CiudadId] INT NOT NULL,
        [TipoSiniestroId] INT NOT NULL,
        [NumeroVictimas] INT NOT NULL DEFAULT 0,
        [Descripcion] NVARCHAR(MAX) NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [FechaModificacion] DATETIME2 NULL,
        CONSTRAINT [PK_Siniestros] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Siniestros_Departamentos] FOREIGN KEY ([DepartamentoId]) 
            REFERENCES [dbo].[Departamentos] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Siniestros_Ciudades] FOREIGN KEY ([CiudadId]) 
            REFERENCES [dbo].[Ciudades] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Siniestros_TiposSiniestro] FOREIGN KEY ([TipoSiniestroId]) 
            REFERENCES [dbo].[TiposSiniestro] ([Id]) ON DELETE NO ACTION
    );
    
    PRINT 'Tabla Siniestros creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla Siniestros ya existe.';
END
GO

-- =============================================
-- Tabla: VehiculosInvolucrados
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VehiculosInvolucrados]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[VehiculosInvolucrados] (
        [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [SiniestroId] UNIQUEIDENTIFIER NOT NULL,
        [Tipo] NVARCHAR(50) NOT NULL, -- Automóvil, Motocicleta, Camión, etc.
        [Placa] NVARCHAR(20) NOT NULL,
        [Marca] NVARCHAR(100) NOT NULL,
        [Modelo] NVARCHAR(100) NOT NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_VehiculosInvolucrados] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_VehiculosInvolucrados_Siniestros] FOREIGN KEY ([SiniestroId]) 
            REFERENCES [dbo].[Siniestros] ([Id]) ON DELETE CASCADE
    );
    
    PRINT 'Tabla VehiculosInvolucrados creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla VehiculosInvolucrados ya existe.';
END
GO
