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