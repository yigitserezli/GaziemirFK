using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gfk.Mvc.Models.Entities
{
	public class PaymentEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public PlayerEntity Player { get; set; } = default!;

		[Required]
		public int Year { get; set; }

		[Required]
		public int Month { get; set; }

		[Required]
		public bool IsPaid { get; set; }

		[Required]
		public string Note { get; set; } = string.Empty;
	}
}

