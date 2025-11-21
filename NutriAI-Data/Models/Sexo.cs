using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class Sexo
{
    public int IdSexo { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
