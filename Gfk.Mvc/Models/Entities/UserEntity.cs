using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;

namespace Gfk.Mvc.Models.Entities
{
    [Index(nameof(Email), IsUnique = true, Name = "IX_UNIQUE_Username")]
    public class UserEntity
	{
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MinLength(3)]
        public string Name { get; set; } = string.Empty;

        [Required, MinLength(2)]
        public string Surname { get; set; } = string.Empty;

        [Required, Phone, MinLength(10), MaxLength(10)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan gerekli"), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan gerekli"), DataType(DataType.Password), MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan gerekli"), DataType(DataType.Password), MinLength(8)]
        public string PasswordConfirm { get; set; } = string.Empty;

        public bool KVKK { get; set; }

        public string ActivationCode { get; set; } = string.Empty;

        public string CancelationCode { get; set; } = string.Empty;
    }
}

