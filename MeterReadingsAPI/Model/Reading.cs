using System.ComponentModel.DataAnnotations;

namespace MeterReadingsAPI.Model
{
	public class Reading
	{
		[Key] public int Key { get; set; }
		public required int AccountId { get; set; }
		public required DateTime MeterReadingDateTime { get; set; }
		public required int MeterReadValue { get; set; }
	}
}
