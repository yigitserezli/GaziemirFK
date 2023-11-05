using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gfk.Mvc.Models.Entities;

namespace Gfk.Mvc.Models
{
	public class ActivationAccountViewModel
	{
		public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

		[Required]
		[MinLength(6), MaxLength(6)]
		public string verificationCode { get; set; } = string.Empty;

        public int UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public UserEntity? User { get; set; }
	}
}

