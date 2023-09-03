using System.ComponentModel.DataAnnotations;

namespace TheTreasure
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MapPoint> MapPoints { get; set; }
    }

    public class MapPoint
    {
        [Key]
        public int Id { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}