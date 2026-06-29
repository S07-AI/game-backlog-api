using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameBacklog.Data;

namespace GameBacklog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            var totalGames = await _context.Games.CountAsync();

            var gamesByStatus = await _context.Games.GroupBy(g=> g.Status).Select(group=> new {Status = group.Key, Count = group.Count()}).ToListAsync();

            var averageRating = await _context.Games
                .Where(g => g.Status == "Completed")
                .AverageAsync(g => g.Rating);

            var totalHours = await _context.Games.SumAsync(g => g.HoursPlayed);

            var mostPlayedPlatform = await _context.Games
                .GroupBy(g => g.PlatformId)
                .Select(group => new { PlatformId = group.Key, Count = group.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefaultAsync();

        
            return Ok(new
            {
                TotalGames = totalGames,
                GamesByStatus = gamesByStatus,
                AverageRatingOfCompleted = averageRating,
                TotalHoursPlayed = totalHours,
                MostPopularPlatformId = mostPlayedPlatform
            });
        }
    }
}