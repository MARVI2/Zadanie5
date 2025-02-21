using Microsoft.EntityFrameworkCore;
using ZADANIE5finalfinal.Models;

namespace ZADANIE5finalfinal.Data
{
	public class ApplicationDBContext:DbContext
	{
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
		{
			
		}

		public DbSet<Klient> Klient { get; set; }
	}
}
