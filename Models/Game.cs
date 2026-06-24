using System.ComponentModel.DataAnnotations;

namespace GameBacklog.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required] //Data Annotations
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Platform { get; set; } = string.Empty;

        [Required]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Playing|Completed|Dropped|Wishlist")]
        public string Status { get; set; } = string.Empty;

        [Range(0,20000)]
        public int HoursPlayed { get; set; }
        
        [Range(1,10)]
        public int Rating { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    }
}