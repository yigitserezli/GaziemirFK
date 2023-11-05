using System;
namespace Gfk.Mvc.Models.Entities
{
	public class ContactFormEntity
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;
    }
}

