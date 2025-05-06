using MeterReadingsAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingsAPI.Data
{
	public class MeterReadingsDbContext(DbContextOptions<MeterReadingsDbContext> options) : DbContext(options)
	{
		public DbSet<Customer> Customers => Set<Customer>();
		public DbSet<Reading> Readings => Set<Reading>();
	}
}
