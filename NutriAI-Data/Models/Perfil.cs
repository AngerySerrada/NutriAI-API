using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class Perfil
{
    public int IdPerfil { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<UsuarioPerfil> UsuarioPerfils { get; set; } = new List<UsuarioPerfil>();
}
