using System;
using System.Collections.Generic;

namespace NutriAI_Data.Models;

public partial class RecetasPdfDetalle
{
    public int IdDetalle { get; set; }

    public int IdRecetaPdf { get; set; }

    public int IdReceta { get; set; }

    public int Orden { get; set; }
}
