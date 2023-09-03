using Microsoft.EntityFrameworkCore;

namespace TheTreasure.Contexts;

public class DBContexts : DbContext
{
    public DBContexts(DbContextOptions<DBContexts> options) : base(options)
    {
        
    }
    public DbSet<Team> Teams { get; set; }
    public DbSet<MapPoint> MapPoints { get; set; }
}