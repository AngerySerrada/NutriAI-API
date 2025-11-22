CREATE TABLE PdfDocuments (
    IdPdfDocument INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    FileName NVARCHAR(255) NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500) NULL,
    FileSize BIGINT NOT NULL,
    ContentType NVARCHAR(50) NOT NULL DEFAULT 'application/pdf',
    PdfContent VARBINARY(MAX) NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IdConversation INT NULL,
    
    -- Índices
    INDEX IX_PdfDocuments_IdUsuario (IdUsuario),
    INDEX IX_PdfDocuments_FechaCreacion (FechaCreacion),
    INDEX IX_PdfDocuments_IdConversation (IdConversation),
    
    -- Relaciones (ajustar según su esquema)
    FOREIGN KEY (IdUsuario) REFERENCES Entidad.Usuarios(IdUsuario),
    FOREIGN KEY (IdConversation) REFERENCES [dbo].[Conversaciones](IdConversacion)
);
select *
from
PdfDocuments