namespace LibraryAPI.Interfaces
{
    public interface ILibrary
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Genre { get; set; }
        public decimal Rating { get; set; }
        public int YearReleased { get; set; }
    }
}