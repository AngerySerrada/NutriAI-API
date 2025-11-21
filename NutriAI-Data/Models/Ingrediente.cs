using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class Ingrediente
{
    public int IdIngrediente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public decimal Calorias { get; set; }

    public decimal Proteinas { get; set; }

    public decimal Carbohidratos { get; set; }

    public decimal Grasas { get; set; }

    public virtual ICollection<UsuarioIngrediente> UsuarioIngredientes { get; set; } = new List<UsuarioIngrediente>();
}
