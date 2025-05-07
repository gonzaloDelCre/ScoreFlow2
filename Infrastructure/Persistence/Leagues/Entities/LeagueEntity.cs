using Infrastructure.Persistence.Standings.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Persistence.Leagues.Entities
{
    [Table("Leagues")]
    public class LeagueEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<StandingEntity> Standings { get; set; } = new List<StandingEntity>();
    }
}

