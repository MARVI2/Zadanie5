using System.ComponentModel.DataAnnotations;

namespace ZADANIE5finalfinal.Models
{
	public class Klient
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Surname { set; get; }
		[Required]
		public string PESEL { get; set; }
		public int BirthYear { get; set; }
		public int Płeć { get; set; }
	}
}
