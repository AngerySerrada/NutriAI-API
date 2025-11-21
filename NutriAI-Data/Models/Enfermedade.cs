using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class Enfermedade
{
    public int IdEnfermedad { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<UsuarioEnfermedad> UsuarioEnfermedads { get; set; } = new List<UsuarioEnfermedad>();
}
