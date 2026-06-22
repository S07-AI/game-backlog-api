namespace GameBacklog.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int HoursPlayed { get; set; }
        public int Rating { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    }
}