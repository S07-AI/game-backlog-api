using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameBacklog.Data;
using GameBacklog.Models;

namespace GameBacklog.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlatformsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Platform>>> GetPlatforms()
        {
            return await _context.Platforms.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Platform>> CreatePlatform(Platform platform)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Platforms.Add(platform);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlatforms), new {id = platform.Id}, platform);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlatform(int id, Platform platform)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != platform.Id)
                return BadRequest();

            _context.Entry(platform).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Platforms.AnyAsync(p => p.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpGet("{id}/games")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByPlatform(int id)
        {
            var platform = await _context.Platforms.FindAsync(id);

            if (platform == null)
                return NotFound();

            return await _context.Games.Where(g=> g.PlatformId == id).ToListAsync();
        }
    }
}