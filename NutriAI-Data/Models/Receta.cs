using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class Receta
{
    public int IdReceta { get; set; }

    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Instrucciones { get; set; } = null!;

    public int TiempoPreparacion { get; set; }

    public int Porciones { get; set; }

    public decimal CaloriasTotales { get; set; }

    public decimal ProteinasTotales { get; set; }

    public decimal CarbohidratosTotales { get; set; }

    public decimal GrasasTotales { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public bool Activo { get; set; }

    public bool EsPublica { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
