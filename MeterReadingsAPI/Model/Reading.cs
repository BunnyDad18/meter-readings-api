using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MeterReadingsAPI.Model
{
	public class Reading
	{
		[Key] public int Key { get; set; }
		public required int AccountId { get; set; }
		public required DateTime MeterReadingDateTime { get; set; }
		public required int MeterReadValue { get; set; }
		[JsonIgnore][ForeignKey("AccountId")] public Customer? Customer { get; set; }
	}
}
