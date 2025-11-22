using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class RecetasPdf
{
    public int IdRecetaPdf { get; set; }

    public int IdUsuario { get; set; }

    public string NombrePdf { get; set; } = null!;

    public byte[] ArchivoPdf { get; set; } = null!;

    public long TamanoBytes { get; set; }

    public DateTime FechaGeneracion { get; set; }

    public int NumeroRecetas { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
