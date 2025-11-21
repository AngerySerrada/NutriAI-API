using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Core.DTOs.Auth
{
    public sealed class ChangePasswordRequest
    {
        public string Login { get; set; } = default!;
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
