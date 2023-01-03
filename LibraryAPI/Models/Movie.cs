﻿using LibraryAPI.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    public class Movie : ILibrary
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public int YearReleased { get; set; }
        public string Director { get; set; } = string.Empty;

    }
}
