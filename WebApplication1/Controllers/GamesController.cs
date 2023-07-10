using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController: ControllerBase
    {
        private readonly DbConfig config;

        public GamesController(DbConfig config)
        {
            this.config = config;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Game>> Get(int id)
        {
            return await config.Games.Include(x=>x.Company).
                FirstOrDefaultAsync(x=>x.Id== id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Game game)
        {
            var gameExist = await config.Companies.AnyAsync(x => x.Id == game.CompanyId);

            if(!gameExist)
            {
                return BadRequest($"No existe la compañía con ID {game.CompanyId}");

            }

            config.Add(game);
            await config.SaveChangesAsync();

            return Ok();

            

        }

    }
}
