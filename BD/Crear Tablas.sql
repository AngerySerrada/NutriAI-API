CREATE SCHEMA Maestro;
GO

CREATE TABLE Entidad.Comunas (
    IdComuna INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1
);

CREATE TABLE Entidad.NivelesActividad (
    IdNivelActividad INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(150) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1
);

CREATE TABLE Entidad.Sexos (
    IdSexo INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(50) NOT NULL
);


CREATE TABLE Entidad.Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,

    -- Datos personales
    Nombre NVARCHAR(150) NOT NULL,
    Correo NVARCHAR(150) NOT NULL UNIQUE,
    Edad INT NOT NULL,

    IdComuna INT NOT NULL,
    IdNivelActividad INT NOT NULL,
    IdSexo INT NOT NULL,

    FechaNacimiento DATE NOT NULL,
    Altura DECIMAL(5,2) NOT NULL,  -- Ej: 175.50 cm
    Peso DECIMAL(5,2) NOT NULL,    -- Ej: 78.40 kg

    -- Credenciales
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARBINARY(256) NOT NULL, 
    PasswordSalt VARBINARY(256) NOT NULL,

    -- Auditoría
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NULL,
    Activo BIT NOT NULL DEFAULT 1,

    -- Relaciones
    CONSTRAINT FK_Usuarios_Comuna FOREIGN KEY (IdComuna) REFERENCES Entidad.Comunas(IdComuna),
    CONSTRAINT FK_Usuarios_NivelActividad FOREIGN KEY (IdNivelActividad) REFERENCES Entidad.NivelesActividad(IdNivelActividad),
    CONSTRAINT FK_Usuarios_Sexo FOREIGN KEY (IdSexo) REFERENCES Entidad.Sexos(IdSexo)
);


CREATE TABLE Maestro.Perfil(
	IdPerfil INT IDENTITY(1,1) PRIMARY KEY,
	Descripcion VARCHAR(100)
)

CREATE TABLE Maestro.UsuarioPerfil (
    IdUsuario INT NOT NULL,
    IdPerfil INT NOT NULL,

    FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_UsuarioPerfil PRIMARY KEY (IdUsuario, IdPerfil),

    CONSTRAINT FK_UsuarioPerfil_Usuario 
        FOREIGN KEY (IdUsuario) REFERENCES Entidad.Usuarios(IdUsuario),

    CONSTRAINT FK_UsuarioPerfil_Perfil 
        FOREIGN KEY (IdPerfil) REFERENCES Maestro.Perfil(IdPerfil)
);


CREATE TABLE Maestro.Ingredientes (
    IdIngrediente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Categoria NVARCHAR(100) NOT NULL,  
    Calorias DECIMAL(10,2) NOT NULL,       
    Proteinas DECIMAL(10,2) NOT NULL,
    Carbohidratos DECIMAL(10,2) NOT NULL,
    Grasas DECIMAL(10,2) NOT NULL
);

CREATE TABLE Entidad.UsuarioIngredientes (
    IdUsuarioIngrediente INT IDENTITY(1,1) PRIMARY KEY,

    IdUsuario INT NOT NULL,
    IdIngrediente INT NOT NULL,

    CantidadGramos DECIMAL(10,2) NULL,  -- opcional (por ejemplo, 150 g)
    FechaRegistro DATETIME NOT NULL DEFAULT(GETDATE()),

    CONSTRAINT FK_UsuarioIngredientes_Usuarios
        FOREIGN KEY (IdUsuario) REFERENCES Entidad.Usuarios(IdUsuario),

    CONSTRAINT FK_UsuarioIngredientes_Ingredientes
        FOREIGN KEY (IdIngrediente) REFERENCES Maestro.Ingredientes(IdIngrediente)
);


CREATE TABLE [Maestro].[Enfermedades] (
    IdEnfermedad INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(200) NOT NULL,
    Descripcion VARCHAR(500) NULL
);

CREATE TABLE [Entidad].[UsuarioEnfermedad] (
    IdUsuarioEnfermedad INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    IdEnfermedad INT NOT NULL,
    FechaDiagnostico DATE NULL,
    Observaciones VARCHAR(500) NULL,
    CONSTRAINT FK_UsuarioEnfermedad_Usuario
        FOREIGN KEY (IdUsuario) REFERENCES [Entidad].[Usuarios](IdUsuario),
    CONSTRAINT FK_UsuarioEnfermedad_Enfermedad
        FOREIGN KEY (IdEnfermedad) REFERENCES [Maestro].[Enfermedades](IdEnfermedad)
);

