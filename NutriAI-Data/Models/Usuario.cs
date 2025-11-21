using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int Edad { get; set; }

    public int IdComuna { get; set; }

    public int IdNivelActividad { get; set; }

    public int IdSexo { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public decimal Altura { get; set; }

    public decimal Peso { get; set; }

    public string Username { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public bool Activo { get; set; }

    public virtual Comuna IdComunaNavigation { get; set; } = null!;

    public virtual NivelesActividad IdNivelActividadNavigation { get; set; } = null!;

    public virtual Sexo IdSexoNavigation { get; set; } = null!;

    public virtual ICollection<UsuarioEnfermedad> UsuarioEnfermedads { get; set; } = new List<UsuarioEnfermedad>();

    public virtual ICollection<UsuarioIngrediente> UsuarioIngredientes { get; set; } = new List<UsuarioIngrediente>();

    public virtual ICollection<UsuarioPerfil> UsuarioPerfils { get; set; } = new List<UsuarioPerfil>();
}
