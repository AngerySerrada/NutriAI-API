using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Core.DTOs.Auth
{
    public sealed class ForgotPasswordRequest
    {
        public string EmailOrLogin { get; set; } = default!;
    }
}
