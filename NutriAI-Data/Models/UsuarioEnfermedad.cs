using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class UsuarioEnfermedad
{
    public int IdUsuarioEnfermedad { get; set; }

    public int IdUsuario { get; set; }

    public int IdEnfermedad { get; set; }

    public DateOnly? FechaDiagnostico { get; set; }

    public string? Observaciones { get; set; }

    public virtual Enfermedade IdEnfermedadNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
