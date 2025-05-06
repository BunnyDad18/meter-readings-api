using MeterReadingsAPI.Data;
using MeterReadingsAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReadingsController : ControllerBase
	{
		private readonly MeterReadingsDbContext _context;
		private MeterReadingsFileReader _csvFileHandler;

		public ReadingsController(MeterReadingsDbContext context)
		{
			_context = context;
			_csvFileHandler = new MeterReadingsFileReader(_context);
		}

		[HttpPost("meter-reading-uploads")]
		public async Task<ActionResult<CsvUploadResult>> UploadReadingsCSV()
		{
			CsvUploadResult uploadResult = new();
			if (!Request.Form.Files.Any())
			{
				Console.WriteLine("No files uploaded.");
				return Ok(uploadResult); 
			}

			foreach(IFormFile file in Request.Form.Files)
			{
				CsvUploadResult fileResult = _csvFileHandler.Process(file);
				uploadResult.Successful += fileResult.Successful;
				uploadResult.Failed += fileResult.Failed;
			}

			await _context.SaveChangesAsync();

			return Ok(uploadResult);
		}

		[HttpGet("get-readings")]
		public async Task<ActionResult<List<Reading>>> GetReadings()
		{
			return Ok(await _context.Readings.ToListAsync());
		}

		[HttpDelete("delete-all-readings")]
		public async Task<ActionResult<int>> DeleteReadings()
		{
			return Ok(await _context.Readings.ExecuteDeleteAsync());
		}

		[HttpGet("get-customers")]
		public async Task<ActionResult<List<Customer>>> GetCustomers()
		{
			return Ok(await _context.Customers.Include(c => c.Readings).ToListAsync());
		}
	}
}
