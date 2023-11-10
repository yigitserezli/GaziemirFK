using System;
namespace Gfk.Mvc.Models
{
	public class PlayerPaginationViewModel<T>
	{
        //Pagination
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        //Filter and Search
        public string SearchString { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Licance { get; set; } = string.Empty;
        public string Foot { get; set; } = string.Empty;
        public string IdentificationNumber { get; set; } = string.Empty;
        public bool? Kvkk { get; set; }
    }
}

