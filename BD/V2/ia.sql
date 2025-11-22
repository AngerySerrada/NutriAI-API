-- ============================================
-- Script para crear tablas de Historial de Consultas IA y PDFs
-- Base de datos: NutriAI
-- Fecha: 2025
-- ============================================

USE NutriAI;
GO



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Recetas]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Recetas] (
        [IdReceta] INT IDENTITY(1,1) NOT NULL,
        [IdUsuario] INT NOT NULL, -- Recetas pueden ser personalizadas por usuario
        [Nombre] NVARCHAR(200) NOT NULL,
        [Descripcion] NVARCHAR(1000) NULL,
        [Instrucciones] NVARCHAR(MAX) NOT NULL,
        [TiempoPreparacion] INT NOT NULL, -- En minutos
        [Porciones] INT NOT NULL DEFAULT 1,
        [CaloriasTotales] DECIMAL(10,2) NOT NULL DEFAULT 0,
        [ProteinasTotales] DECIMAL(10,2) NOT NULL DEFAULT 0,
        [CarbohidratosTotales] DECIMAL(10,2) NOT NULL DEFAULT 0,
        [GrasasTotales] DECIMAL(10,2) NOT NULL DEFAULT 0,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [FechaActualizacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [EsPublica] BIT NOT NULL DEFAULT 0, -- Puede ser compartida con otros usuarios
        
        CONSTRAINT [PK_Recetas] PRIMARY KEY CLUSTERED ([IdReceta] ASC),
        CONSTRAINT [FK_Recetas_Usuarios] FOREIGN KEY ([IdUsuario]) 
            REFERENCES [Entidad].[Usuarios]([IdUsuario]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_Recetas_IdUsuario] ON [dbo].[Recetas] ([IdUsuario]);
    CREATE INDEX [IX_Recetas_Nombre] ON [dbo].[Recetas] ([Nombre]);
    CREATE INDEX [IX_Recetas_FechaCreacion] ON [dbo].[Recetas] ([FechaCreacion] DESC);
    
    PRINT 'Tabla [Recetas] creada exitosamente.';
END
ELSE
    PRINT 'Tabla [Recetas] ya existe.';
GO

-- ============================================
-- Tabla: Conversaciones (Historial de consultas con IA)
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Conversaciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Conversaciones] (
        [IdConversacion] INT IDENTITY(1,1) NOT NULL,
        [IdUsuario] INT NOT NULL,
        [Titulo] NVARCHAR(200) NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [FechaActualizacion] DATETIME2 NULL,
        [Activa] BIT NOT NULL DEFAULT 1,
        
        CONSTRAINT [PK_Conversaciones] PRIMARY KEY CLUSTERED ([IdConversacion] ASC),
        CONSTRAINT [FK_Conversaciones_Usuarios] FOREIGN KEY ([IdUsuario]) 
            REFERENCES [Entidad].[Usuarios]([IdUsuario]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_Conversaciones_IdUsuario] ON [dbo].[Conversaciones] ([IdUsuario]);
    CREATE INDEX [IX_Conversaciones_FechaCreacion] ON [dbo].[Conversaciones] ([FechaCreacion] DESC);
    
    PRINT 'Tabla [Conversaciones] creada exitosamente.';
END
ELSE
    PRINT 'Tabla [Conversaciones] ya existe.';
GO

-- ============================================
-- Tabla: MensajesConversacion (Mensajes individuales del chat)
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MensajesConversacion]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[MensajesConversacion] (
        [IdMensaje] INT IDENTITY(1,1) NOT NULL,
        [IdConversacion] INT NOT NULL,
        [Rol] VARCHAR(20) NOT NULL, -- 'user' o 'assistant'
        [Contenido] NVARCHAR(MAX) NOT NULL,
        [FechaEnvio] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [TokensUtilizados] INT NULL,
        [ContextoIncluido] NVARCHAR(MAX) NULL, -- JSON con las opciones de contexto utilizadas
        
        CONSTRAINT [PK_MensajesConversacion] PRIMARY KEY CLUSTERED ([IdMensaje] ASC),
        CONSTRAINT [FK_MensajesConversacion_Conversaciones] FOREIGN KEY ([IdConversacion]) 
            REFERENCES [dbo].[Conversaciones]([IdConversacion]) ON DELETE CASCADE,
        CONSTRAINT [CK_MensajesConversacion_Rol] CHECK ([Rol] IN ('user', 'assistant', 'system'))
    );
    
    CREATE INDEX [IX_MensajesConversacion_IdConversacion] ON [dbo].[MensajesConversacion] ([IdConversacion]);
    CREATE INDEX [IX_MensajesConversacion_FechaEnvio] ON [dbo].[MensajesConversacion] ([FechaEnvio] DESC);
    
    PRINT 'Tabla [MensajesConversacion] creada exitosamente.';
END
ELSE
    PRINT 'Tabla [MensajesConversacion] ya existe.';
GO

-- ============================================
-- Tabla: RecetasPDF (PDFs generados de recetas)
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RecetasPDF]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[RecetasPDF] (
        [IdRecetaPDF] INT IDENTITY(1,1) NOT NULL,
        [IdUsuario] INT NOT NULL,
        [NombrePDF] NVARCHAR(200) NOT NULL,
        [ArchivoPDF] VARBINARY(MAX) NOT NULL, -- Contenido del PDF en binario
        [TamanoBytes] BIGINT NOT NULL,
        [FechaGeneracion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [NumeroRecetas] INT NOT NULL DEFAULT 1,
        
        CONSTRAINT [PK_RecetasPDF] PRIMARY KEY CLUSTERED ([IdRecetaPDF] ASC),
        CONSTRAINT [FK_RecetasPDF_Usuarios] FOREIGN KEY ([IdUsuario]) 
            REFERENCES [Entidad].[Usuarios]([IdUsuario]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_RecetasPDF_IdUsuario] ON [dbo].[RecetasPDF] ([IdUsuario]);
    CREATE INDEX [IX_RecetasPDF_FechaGeneracion] ON [dbo].[RecetasPDF] ([FechaGeneracion] DESC);
    
    PRINT 'Tabla [RecetasPDF] creada exitosamente.';
END
ELSE
    PRINT 'Tabla [RecetasPDF] ya existe.';
GO

-- ============================================
-- Tabla: RecetasPDF_Detalle (Relación muchos a muchos entre PDFs y Recetas)
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RecetasPDF_Detalle]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[RecetasPDF_Detalle] (
        [IdDetalle] INT IDENTITY(1,1) NOT NULL,
        [IdRecetaPDF] INT NOT NULL,
        [IdReceta] INT NOT NULL,
        [Orden] INT NOT NULL DEFAULT 1, -- Orden en que aparece la receta en el PDF
    );
    
    CREATE INDEX [IX_RecetasPDF_Detalle_IdRecetaPDF] ON [dbo].[RecetasPDF_Detalle] ([IdRecetaPDF]);
    CREATE INDEX [IX_RecetasPDF_Detalle_IdReceta] ON [dbo].[RecetasPDF_Detalle] ([IdReceta]);
    
    PRINT 'Tabla [RecetasPDF_Detalle] creada exitosamente.';
END
ELSE
    PRINT 'Tabla [RecetasPDF_Detalle] ya existe.';
GO

-- ============================================
-- Stored Procedures para Conversaciones
-- ============================================

-- SP: Crear nueva conversación
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CrearConversacion]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_CrearConversacion];
GO

CREATE PROCEDURE [dbo].[sp_CrearConversacion]
    @IdUsuario INT,
    @Titulo NVARCHAR(200) = NULL,
    @IdConversacion INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Si no hay título, generar uno por defecto
    IF @Titulo IS NULL OR @Titulo = ''
    BEGIN
        SET @Titulo = 'Consulta ' + CONVERT(VARCHAR, GETUTCDATE(), 103);
    END
    
    INSERT INTO [dbo].[Conversaciones] ([IdUsuario], [Titulo], [FechaCreacion], [Activa])
    VALUES (@IdUsuario, @Titulo, GETUTCDATE(), 1);
    
    SET @IdConversacion = SCOPE_IDENTITY();
    
    SELECT @IdConversacion as IdConversacion, 'Conversación creada exitosamente' as Mensaje;
END
GO

-- SP: Guardar mensaje en conversación
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GuardarMensaje]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_GuardarMensaje];
GO

CREATE PROCEDURE [dbo].[sp_GuardarMensaje]
    @IdConversacion INT,
    @Rol VARCHAR(20),
    @Contenido NVARCHAR(MAX),
    @TokensUtilizados INT = NULL,
    @ContextoIncluido NVARCHAR(MAX) = NULL,
    @IdMensaje INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validar que la conversación existe
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Conversaciones] WHERE [IdConversacion] = @IdConversacion)
    BEGIN
        RAISERROR('La conversación no existe', 16, 1);
        RETURN;
    END
    
    -- Insertar el mensaje
    INSERT INTO [dbo].[MensajesConversacion] 
        ([IdConversacion], [Rol], [Contenido], [FechaEnvio], [TokensUtilizados], [ContextoIncluido])
    VALUES 
        (@IdConversacion, @Rol, @Contenido, GETUTCDATE(), @TokensUtilizados, @ContextoIncluido);
    
    SET @IdMensaje = SCOPE_IDENTITY();
    
    -- Actualizar fecha de actualización de la conversación
    UPDATE [dbo].[Conversaciones]
    SET [FechaActualizacion] = GETUTCDATE()
    WHERE [IdConversacion] = @IdConversacion;
    
    SELECT @IdMensaje as IdMensaje, 'Mensaje guardado exitosamente' as Mensaje;
END
GO

-- SP: Obtener historial de conversación
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ObtenerHistorialConversacion]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ObtenerHistorialConversacion];
GO

CREATE PROCEDURE [dbo].[sp_ObtenerHistorialConversacion]
    @IdConversacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        m.[IdMensaje],
        m.[IdConversacion],
        m.[Rol],
        m.[Contenido],
        m.[FechaEnvio],
        m.[TokensUtilizados],
        m.[ContextoIncluido]
    FROM [dbo].[MensajesConversacion] m
    WHERE m.[IdConversacion] = @IdConversacion
    ORDER BY m.[FechaEnvio] ASC;
END
GO

-- SP: Obtener conversaciones de usuario
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ObtenerConversacionesUsuario]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ObtenerConversacionesUsuario];
GO

CREATE PROCEDURE [dbo].[sp_ObtenerConversacionesUsuario]
    @IdUsuario INT,
    @SoloActivas BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        c.[IdConversacion],
        c.[IdUsuario],
        c.[Titulo],
        c.[FechaCreacion],
        c.[FechaActualizacion],
        c.[Activa],
        (SELECT COUNT(*) FROM [dbo].[MensajesConversacion] WHERE [IdConversacion] = c.[IdConversacion]) as TotalMensajes,
        (SELECT TOP 1 [Contenido] FROM [dbo].[MensajesConversacion] 
         WHERE [IdConversacion] = c.[IdConversacion] AND [Rol] = 'user'
         ORDER BY [FechaEnvio] ASC) as PrimerMensaje
    FROM [dbo].[Conversaciones] c
    WHERE c.[IdUsuario] = @IdUsuario
        AND (@SoloActivas = 0 OR c.[Activa] = 1)
    ORDER BY c.[FechaActualizacion] DESC, c.[FechaCreacion] DESC;
END
GO

-- ============================================
-- Stored Procedures para PDFs de Recetas
-- ============================================

-- SP: Guardar PDF de recetas
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GuardarRecetaPDF]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_GuardarRecetaPDF];
GO

CREATE PROCEDURE [dbo].[sp_GuardarRecetaPDF]
    @IdUsuario INT,
    @NombrePDF NVARCHAR(200),
    @ArchivoPDF VARBINARY(MAX),
    @TamanoBytes BIGINT,
    @RecetasIds NVARCHAR(MAX), -- IDs separados por comas: "1,2,3"
    @IdRecetaPDF INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Contar número de recetas
        DECLARE @NumeroRecetas INT;
        SELECT @NumeroRecetas = COUNT(*) 
        FROM STRING_SPLIT(@RecetasIds, ',');
        
        -- Insertar el registro del PDF
        INSERT INTO [dbo].[RecetasPDF] 
            ([IdUsuario], [NombrePDF], [ArchivoPDF], [TamanoBytes], [FechaGeneracion], [NumeroRecetas])
        VALUES 
            (@IdUsuario, @NombrePDF, @ArchivoPDF, @TamanoBytes, GETUTCDATE(), @NumeroRecetas);
        
        SET @IdRecetaPDF = SCOPE_IDENTITY();
        
        -- Insertar los detalles de las recetas
        DECLARE @Orden INT = 1;
        DECLARE @IdReceta INT;
        
        DECLARE recetas_cursor CURSOR FOR
        SELECT CAST(value AS INT) FROM STRING_SPLIT(@RecetasIds, ',');
        
        OPEN recetas_cursor;
        FETCH NEXT FROM recetas_cursor INTO @IdReceta;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            INSERT INTO [dbo].[RecetasPDF_Detalle] ([IdRecetaPDF], [IdReceta], [Orden])
            VALUES (@IdRecetaPDF, @IdReceta, @Orden);
            
            SET @Orden = @Orden + 1;
            FETCH NEXT FROM recetas_cursor INTO @IdReceta;
        END
        
        CLOSE recetas_cursor;
        DEALLOCATE recetas_cursor;
        
        COMMIT TRANSACTION;
        
        SELECT @IdRecetaPDF as IdRecetaPDF, 'PDF guardado exitosamente' as Mensaje;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- SP: Obtener PDFs de usuario
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ObtenerPDFsUsuario]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ObtenerPDFsUsuario];
GO

CREATE PROCEDURE [dbo].[sp_ObtenerPDFsUsuario]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.[IdRecetaPDF],
        p.[IdUsuario],
        p.[NombrePDF],
        p.[TamanoBytes],
        p.[FechaGeneracion],
        p.[NumeroRecetas],
        STUFF((
            SELECT ', ' + r.[Nombre]
            FROM [dbo].[RecetasPDF_Detalle] d
            INNER JOIN [dbo].[Recetas] r ON d.[IdReceta] = r.[IdReceta]
            WHERE d.[IdRecetaPDF] = p.[IdRecetaPDF]
            ORDER BY d.[Orden]
            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') as NombresRecetas
    FROM [dbo].[RecetasPDF] p
    WHERE p.[IdUsuario] = @IdUsuario
    ORDER BY p.[FechaGeneracion] DESC;
END
GO

-- SP: Descargar PDF
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DescargarPDF]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_DescargarPDF];
GO

CREATE PROCEDURE [dbo].[sp_DescargarPDF]
    @IdRecetaPDF INT,
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        [IdRecetaPDF],
        [NombrePDF],
        [ArchivoPDF],
        [TamanoBytes],
        [FechaGeneracion]
    FROM [dbo].[RecetasPDF]
    WHERE [IdRecetaPDF] = @IdRecetaPDF 
        AND [IdUsuario] = @IdUsuario;
END
GO

-- SP: Eliminar PDF
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EliminarPDF]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_EliminarPDF];
GO

CREATE PROCEDURE [dbo].[sp_EliminarPDF]
    @IdRecetaPDF INT,
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM [dbo].[RecetasPDF]
    WHERE [IdRecetaPDF] = @IdRecetaPDF 
        AND [IdUsuario] = @IdUsuario;
    
    IF @@ROWCOUNT > 0
        SELECT 1 as Success, 'PDF eliminado exitosamente' as Mensaje;
    ELSE
        SELECT 0 as Success, 'PDF no encontrado o no tiene permisos' as Mensaje;
END
GO

-- ============================================
-- Datos de ejemplo (opcional)
-- ============================================

PRINT '';
PRINT '============================================';
PRINT 'Script completado exitosamente';
PRINT 'Tablas creadas:';
PRINT '  - Conversaciones';
PRINT '  - MensajesConversacion';
PRINT '  - RecetasPDF';
PRINT '  - RecetasPDF_Detalle';
PRINT '';
PRINT 'Stored Procedures creados:';
PRINT '  - sp_CrearConversacion';
PRINT '  - sp_GuardarMensaje';
PRINT '  - sp_ObtenerHistorialConversacion';
PRINT '  - sp_ObtenerConversacionesUsuario';
PRINT '  - sp_GuardarRecetaPDF';
PRINT '  - sp_ObtenerPDFsUsuario';
PRINT '  - sp_DescargarPDF';
PRINT '  - sp_EliminarPDF';
PRINT '============================================';
PRINT '';
GO
