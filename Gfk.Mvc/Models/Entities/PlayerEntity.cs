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

		public string Category { get; set; } = string.Empty;

		[Required]
        [DataType(DataType.Date)]
        public DateTime BornDate { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Note { get; set; } = string.Empty;

        [Required]
        public bool Licance { get; set; }

		[Required]
		[MaxLength(500)]
		public string ParentContact { get; set; } = string.Empty;

	}
}
