using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheTreasure.Contexts;

namespace TheTreasure.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MapPointsController : ControllerBase
    {
	    private readonly DBContexts _context;

        public MapPointsController(DBContexts context)
        {
	        _context = context;
        }

        [HttpGet]
        public Task<IActionResult> Index() => Task.FromResult<IActionResult>(new OkObjectResult(new {message = "I'm fine bro"}));

		[HttpGet]
        public async Task<IEnumerable<object>> Teams() => await _context.Teams.Select(x => new { x.Id, x.Name}).ToListAsync();

        [HttpGet]
        public async Task<IEnumerable<object>> TeamPoints(int teamId) => await _context.MapPoints.Where(x=>x.TeamId == teamId).Select(x=> new
        {
            x.Id,
            x.Latitude,
            x.Longitude,
            x.TeamId,
        }).ToListAsync();
        
        
        [HttpPost]
        public async Task<IActionResult> Team(string teamName)
        {
	       
	        await _context.Teams.AddAsync(new Team { Name = teamName });
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { Status = 200 , Message = "Adding Team Done Successfully"});
        }

        [HttpDelete]
        public async Task<IActionResult> Team(int teamId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(x=>x.Id == teamId);
            if (team == null) return new OkObjectResult(new { Status = 404, Message = "Not found" });
            
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { Status = 200 , Message = "Removing Team Done Successfully"});
        }
        
        [HttpPost]
        public async Task<IActionResult> Points(AddTeamPointsDto teamPoints)
        {
            if (!_context.Teams.Any(x=>x.Id == teamPoints.TeamId))
                return new OkObjectResult(new { Status = 500, Message = "Can't find the team, please add it first" });

            var oldPints = await _context.MapPoints.Where(x => x.TeamId == teamPoints.TeamId).ToListAsync();
            if(oldPints.Any())  _context.MapPoints.RemoveRange(oldPints);
            
            var mapPoints = teamPoints.Points.Select(point => new MapPoint { TeamId = teamPoints.TeamId, Latitude = point.Latitude, Longitude = point.Longitude }).ToList();
            await _context.MapPoints.AddRangeAsync(mapPoints);

            await _context.SaveChangesAsync();
            return new OkObjectResult(new { Status = 200 , Message = $"Adding Points for Team {teamPoints.TeamId} Done Successfully"});
        }


    }
}