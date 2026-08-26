using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class StudentLoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be 3–50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_.\-]+$", ErrorMessage = "Username can only contain letters, numbers, dot, dash and underscore.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
