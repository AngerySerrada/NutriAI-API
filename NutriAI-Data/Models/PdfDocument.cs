using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class PdfDocument
{
    public int IdPdfDocument { get; set; }

    public int IdUsuario { get; set; }

    public string FileName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public long FileSize { get; set; }

    public string ContentType { get; set; } = null!;

    public byte[] PdfContent { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public int? IdConversation { get; set; }

    public virtual Conversacione? IdConversationNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
