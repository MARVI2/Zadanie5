using OfficeOpenXml;
using ZADANIE5finalfinal.Models;

namespace ZADANIE5finalfinal.Services
{
	public class XLSXService
	{
		public IEnumerable<Klient> ReadXLSXFile(Stream filestream)
		{
			List<Klient> klienci = new List<Klient>();

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using (var package = new ExcelPackage(filestream))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

				for(int row = 2; row <= worksheet.Dimension.Rows;row++)
				{
					klienci.Add(new Klient
					{
						Id = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
						Name = worksheet.Cells[row, 2].Value.ToString().Trim(),
						Surname = worksheet.Cells[row, 3].Value.ToString().Trim(),
						PESEL = worksheet.Cells[row, 4].Value.ToString().Trim(),
						BirthYear = int.Parse(worksheet.Cells[row, 5].Value.ToString().Trim()),
						Płeć = int.Parse(worksheet.Cells[row, 6].Value.ToString().Trim())
					});
				}

			}
			return klienci;
		}
	}
}
