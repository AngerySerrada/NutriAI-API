using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class MensajesConversacion
{
    public int IdMensaje { get; set; }

    public int IdConversacion { get; set; }

    public string Rol { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public DateTime FechaEnvio { get; set; }

    public int? TokensUtilizados { get; set; }

    public string? ContextoIncluido { get; set; }

    public virtual Conversacione IdConversacionNavigation { get; set; } = null!;
}
