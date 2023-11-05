using System;
using System.ComponentModel.DataAnnotations;

namespace Gfk.Mvc.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Bu alan gerekli"), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan gerekli"), DataType(DataType.Password), MinLength(8)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }

    }
}

