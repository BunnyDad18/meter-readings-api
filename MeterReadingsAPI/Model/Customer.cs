using System.ComponentModel.DataAnnotations;

namespace MeterReadingsAPI.Model
{
	public class Customer
	{
		[Key] public int AccountId { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public ICollection<Reading>? Readings { get; set; }
	}
}
