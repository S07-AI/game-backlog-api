using System.ComponentModel.DataAnnotations;
using GameBacklog.Models;

namespace GameBacklog.Models
{
    public class Platform
    {
        public int Id { get; set;}

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name {get; set;} = string.Empty;

        public ICollection<Game> Games {get; set; } = new List<Game>();
    }
}