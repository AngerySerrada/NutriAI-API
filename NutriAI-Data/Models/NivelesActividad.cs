using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class NivelesActividad
{
    public int IdNivelActividad { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
