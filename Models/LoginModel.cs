using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be 3–100 characters.")]
        // 👈 ab email allow karta hai (Registrar ke liye letters/numbers/dot/dash/underscore bhi chalega, Student ke liye email bhi)
        [RegularExpression(@"^[a-zA-Z0-9_.\-@]+$", ErrorMessage = "Username contains invalid characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Password must be at least 3 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}