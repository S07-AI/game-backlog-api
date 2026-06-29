using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameBacklog.Data;
using GameBacklog.Models;

namespace GameBacklog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GamesController(AppDbContext context) //Dependency Injection
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames([FromQuery] string? status, [FromQuery] int? platform, [FromQuery] string? genre, [FromQuery] string? sort)
        {
            var query = _context.Games.Include(g => g.Platform).AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(g=> g.Status == status);

            if (platform.HasValue)
                query = query.Where(g=> g.PlatformId == platform);
            
            if (!string.IsNullOrEmpty(genre))
                query = query.Where(g=> g.Genre == genre);
            
            if (sort == "rating")
                query = query.OrderByDescending(g=> g.Rating);
            else if(sort == "hoursPlayed")
                query = query.OrderByDescending(g=> g.HoursPlayed);

            return await query.ToListAsync();//go to the Games table in PostgreSQL, grab everything, and return it as a list.
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.Include(g => g.Platform).FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) 
                return NotFound();

            return game;
        }

        [HttpPost]
        public async Task<ActionResult<Game>> CreateGame(Game game)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(g => g.Id == id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, Game game)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            if (id != game.Id)
                return BadRequest();

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound();

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Game>>> SearchGames([FromQuery] string q)
        {
            if (string.IsNullOrEmpty(q))
                return BadRequest("Search query cannot be empty");

            var results = await _context.Games.Include(g => g.Platform).Where(g=> g.Title.ToLower().Contains(q.ToLower())).ToListAsync();

            return results;
        }
    }
}