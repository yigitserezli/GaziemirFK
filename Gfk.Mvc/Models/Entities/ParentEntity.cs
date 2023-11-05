using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gfk.Mvc.Models.Entities
{
	public class ParentEntity
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

        [MinLength(11), MaxLength(11)]
        public string IdentificationNumber { get; set; } = string.Empty;

        public string Job { get; set; } = string.Empty;

        public int PlayerId { get; set; }

        [ForeignKey(nameof(PlayerId))]
        public PlayerEntity? Player { get; }
    }
}
