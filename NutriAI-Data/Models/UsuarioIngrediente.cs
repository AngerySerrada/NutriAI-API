using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class UsuarioIngrediente
{
    public int IdUsuarioIngrediente { get; set; }

    public int IdUsuario { get; set; }

    public int IdIngrediente { get; set; }

    public decimal? CantidadGramos { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Ingrediente IdIngredienteNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
