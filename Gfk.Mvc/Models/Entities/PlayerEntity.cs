using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gfk.Mvc.Models.Entities
{
    public class PlayerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(55)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(55)]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(11), MaxLength(11)]
        public string IdentificationNumber { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

		[Required]
        [DataType(DataType.Date)]
        public DateTime BornDate { get; set; }

        [MaxLength(500)]
        public string Note { get; set; } = string.Empty;

        [Required]
        public string Licance { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Address = string.Empty;

        [Required]
        public string Foot { get; set; } = string.Empty;

        public DateTime UptadetAt { get; set; }

        public bool MailPermission { get; set; }

        public bool SmsPermission { get; set; }

        
    }
}
