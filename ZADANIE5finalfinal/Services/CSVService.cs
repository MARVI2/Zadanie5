using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;
using System.IO;
using ZADANIE5finalfinal.Models;

namespace ZADANIE5finalfinal.Services
{
	public class CSVService
	{
		public IEnumerable<Klient> ReadCSVFile(Stream filestream)
		{
			using (var reader = new StreamReader(filestream))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var records = csv.GetRecords<Klient>();
				return records.ToList();
			}
		}
	}
}
