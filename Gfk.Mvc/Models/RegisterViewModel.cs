using System;
using System.ComponentModel.DataAnnotations;

namespace Gfk.Mvc.Models
{
    public class RegisterViewModel
    {
        [Required, MinLength(3)]
        public string Name { get; set; } = string.Empty;

        [Required, MinLength(2)]
        public string Surname { get; set; } = string.Empty;

        [Required, Phone, MinLength(10), MaxLength(10)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan gerekli"), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan gerekli"), DataType(DataType.Password), MinLength(8)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Bu alan gerekli"), DataType(DataType.Password), MinLength(8)]
        public string PasswordConfirm { get; set; } = string.Empty;

        public bool KVKK { get; set; }
    }
}

