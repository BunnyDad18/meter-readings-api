using MeterReadingsAPI.Data;
using MeterReadingsAPI.Model;
using System.Linq;

namespace MeterReadingsAPI.Controllers
{
	public class MeterReadingsFileReader
	{
		private MeterReadingsDbContext _context;

		public MeterReadingsFileReader(MeterReadingsDbContext context)
		{
			_context = context;
		}

		public CsvUploadResult Process(IFormFile file)
		{
			CsvUploadResult result = new CsvUploadResult();

			List<Reading> newReadings = new List<Reading>();

			using StreamReader stream = new(file.OpenReadStream());
			stream.ReadLine();
			while (!stream.EndOfStream)
			{
				string line = stream.ReadLine();
				bool valid = ParseReading(line, out Reading reading);
				if (valid && CheckData(reading, newReadings) && CheckData(reading, _context.Readings))
				{
					reading.Key = _context.Readings.Count();
					newReadings.Add(reading);
					result.Successful++;
				}
				else result.Failed++;
			}

			_context.Readings.AddRange(newReadings);

			return result;
		}

		private bool CheckData(Reading newReading, IEnumerable<Reading> readings)
		{
			if (_context.Customers.Find(newReading.AccountId) == null) return false;
			foreach(Reading storedReading in readings)
			{
				if (CheckReading(storedReading, newReading)) return false;
			}
			return true;
		}

		private bool CheckReading(Reading storedReading, Reading newReading)
		{
			if (storedReading.AccountId != newReading.AccountId) return false;
			if (storedReading.MeterReadValue == newReading.MeterReadValue) return true;
			if (storedReading.MeterReadingDateTime > newReading.MeterReadingDateTime) return true;
			return false;
		}

		private bool ParseReading(string input, out Reading reading)
		{
			bool valid = true;
			reading = new Reading() { AccountId = 0, MeterReadingDateTime = new DateTime(), MeterReadValue = 0 };
			string[] values = input.Split(',');
			if (values.Length < 3) return false;

			if (!int.TryParse(values[0], out int accountId)) valid = false;
			if (!DateTime.TryParse(values[1], out DateTime dataTime)) valid = false;
			if (values[2].Length != 5) valid = false;
			if (!int.TryParse(values[2], out int meterValue)) valid = false;

			reading.AccountId = accountId;
			reading.MeterReadingDateTime = dataTime;
			reading.MeterReadValue = meterValue;

			return valid;
		}
	}
}
