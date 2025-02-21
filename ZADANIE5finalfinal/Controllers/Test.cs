using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Formats.Asn1;
using System.Globalization;
using ZADANIE5finalfinal.Data;
using ZADANIE5finalfinal.Models;
using ZADANIE5finalfinal.Services;

namespace ZADANIE5finalfinal.Controllers
{
    public class Test : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly CSVService _CSVService;
        private readonly XLSXService _XLSXService;
		public Test(ApplicationDBContext db, CSVService cSVService, XLSXService xLSXService)
		{
            _db = db;
            _CSVService = cSVService;
            _XLSXService = xLSXService;
		}
		public async Task<IActionResult> Index()
        {
			var klients = await _db.Klient.ToListAsync();
			return View(klients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,PESEL,BirthYear,Płeć")] Klient klient)
        {
            if (ModelState.IsValid)
            {
                await _db.Klient.AddAsync(klient);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(klient);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var klient = await _db.Klient.FindAsync(id);
            return View(klient);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Name,Surname,PESEL,BirthYear,Płeć")] Klient klient)
        {
            if (ModelState.IsValid)
            {
                _db.Klient.Update(klient);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(klient);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var klient = await _db.Klient.FindAsync(id);
            return View(klient);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([Bind("Id,Name,Surname,PESEL,BirthYear,Płeć")] Klient klient)
        {
            _db.Klient.Remove(klient);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Produces("text/cvs")]
        public async Task<IActionResult> DownloadCVS()
        {
            var data = await _db.Klient.ToListAsync();

            using(var memoryStream = new MemoryStream())
            {
				using (var streamWriter = new StreamWriter(memoryStream))
				using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
				{
					csvWriter.WriteRecords(data);
				}
				return File(memoryStream.ToArray(), "text/csv", $"Export-{DateTime.Now.ToString("s")}.cvs");
			}
        }

        [HttpPost]
        
        public async Task<IActionResult> DownloadXLSX()
        {
            var klienci = await _db.Klient.ToListAsync();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using(var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products");

                worksheet.Cells["A1"].Value = "Id";
				worksheet.Cells["B1"].Value = "Name";
				worksheet.Cells["C1"].Value = "Surname";
				worksheet.Cells["D1"].Value = "PESEL";
				worksheet.Cells["E1"].Value = "BirthYear";
				worksheet.Cells["F1"].Value = "Płeć";

                for(int i =0;i<klienci.Count();i++)
                {
                    worksheet.Cells[i + 2, 1].Value = klienci[i].Id;
					worksheet.Cells[i + 2, 2].Value = klienci[i].Name;
					worksheet.Cells[i + 2, 3].Value = klienci[i].Surname;
					worksheet.Cells[i + 2, 4].Value = klienci[i].PESEL;
					worksheet.Cells[i + 2, 5].Value = klienci[i].BirthYear;
					worksheet.Cells[i + 2, 6].Value = klienci[i].Płeć;
				}

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Klienci.xlsx");
			}
        }

        [HttpPost]
        public async Task CsvImport(IFormFile csvFile)
        {
            if(csvFile != null && csvFile.Length > 0)
            {
                using (var stream = csvFile.OpenReadStream())
                {
                     List <Klient> klienci = _CSVService.ReadCSVFile(stream).ToList();
                    foreach (var klient in klienci)
                    {
                        if (_db.Klient.FirstOrDefault(id =>id.Id == klient.Id) == null)
                        {
                            _db.Klient.Add(klient);
                        }
                    }
                    await _db.SaveChangesAsync();
                }
            }

        }

        public async Task XLSXImport(IFormFile xlsxFile)
        {
            if(xlsxFile != null)
            {
                var stream = new MemoryStream();
                await xlsxFile.CopyToAsync(stream);
                var klienci = _XLSXService.ReadXLSXFile(stream);

				foreach (var klient in klienci)
				{
					if (_db.Klient.FirstOrDefault(id => id.Id == klient.Id) == null)
					{
						_db.Klient.Add(klient);
					}
				}
				await _db.SaveChangesAsync();
			}

        }
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile importFile,string fileType)
        {
            if (importFile.FileName.Split(".")[^1] != "csv" || importFile.FileName.Split(".")[^1] != "xlsx")
            {
                TempData["FileError"] = "Złe rozszserzenir pliku";
                return RedirectToAction("Index");
            }
            if (fileType == "csv")
            {
                await CsvImport(importFile);
            }
            else
            {
                await XLSXImport(importFile);
            }

                return RedirectToAction("Index");
		}
    }
}
