using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class UsuarioPerfil
{
    public int IdUsuario { get; set; }

    public int IdPerfil { get; set; }

    public DateTime FechaAsignacion { get; set; }

    public bool Activo { get; set; }

    public virtual Perfil IdPerfilNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
