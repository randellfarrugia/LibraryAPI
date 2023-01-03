using LibraryAPI.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    public class Book : ILibrary
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public int YearReleased { get; set; }
        public int Pages { get; set; } = 0;
        public string Author { get; set; } = string.Empty;

    }
}
