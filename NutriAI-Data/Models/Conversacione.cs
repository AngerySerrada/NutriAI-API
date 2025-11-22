using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class Conversacione
{
    public int IdConversacion { get; set; }

    public int IdUsuario { get; set; }

    public string? Titulo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public bool Activa { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<MensajesConversacion> MensajesConversacions { get; set; } = new List<MensajesConversacion>();

    public virtual ICollection<PdfDocument> PdfDocuments { get; set; } = new List<PdfDocument>();
}
