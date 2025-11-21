using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class Comuna
{
    public int IdComuna { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
