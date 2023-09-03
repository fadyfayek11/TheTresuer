using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheTreasure.Contexts;

namespace TheTreasure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapPointsController : ControllerBase
    {
        private readonly ILogger<MapPointsController> _logger;
        private readonly DBContexts _context;

        public MapPointsController(ILogger<MapPointsController> logger, DBContexts context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "Teams")]
        public async Task<IEnumerable<Team>> GetTeams() => await _context.Teams.ToListAsync();

        [HttpGet(Name = "TeamPoints")]
        public async Task<IEnumerable<MapPoint>> GetTeamPoints(int teamId) => await _context.MapPoints.Where(x=>x.TeamId == teamId).ToListAsync();
        
        
        [HttpPost(Name = "Team")]
        public async Task<IActionResult> PostTeam(string teamName)
        {
            await _context.Teams.AddAsync(new Team { Name = teamName });
            return new OkObjectResult(new { Status = 200 , Message = "Adding Team Done Successfully"});
        }
        
        [HttpPost(Name = "Points")]
        public async Task<IActionResult> PostPoints(int teamId,Dictionary<string,string> points)
        {
            if (!_context.Teams.Any(x=>x.Id == teamId))
                return new OkObjectResult(new { Status = 500, Message = "Can't find the team, please add it first" });

            var oldPints = await _context.MapPoints.Where(x => x.TeamId == teamId).ToListAsync();
            if(oldPints.Any())  _context.MapPoints.RemoveRange(oldPints);
            
            var mapPoints = points.Select(point => new MapPoint { TeamId = teamId, Latitude = point.Key, Longitude = point.Value }).ToList();
            await _context.MapPoints.AddRangeAsync(mapPoints);
            
            return new OkObjectResult(new { Status = 200 , Message = $"Adding Points for Team {teamId} Done Successfully"});
        }


    }
}