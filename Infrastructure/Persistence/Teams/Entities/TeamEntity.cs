using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Standings.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;

namespace Infrastructure.Persistence.Teams.Entities
{
    [Table("Teams")]
    public class TeamEntity
    {
        [Key]
        public int TeamID { get; set; }

        [MaxLength(100)]
        public string? ExternalID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(100)]
        public string? Club { get; set; }

        [MaxLength(255)]
        public string? Stadium { get; set; }

        [MaxLength(500)]
        public string? Logo { get; set; }

        public int? CoachPlayerID { get; set; }

        [ForeignKey(nameof(CoachPlayerID))]
        public PlayerEntity? Coach { get; set; }

        public ICollection<TeamPlayerEntity> TeamPlayers { get; set; } = new List<TeamPlayerEntity>();
        public ICollection<StandingEntity> Standings { get; set; } = new List<StandingEntity>();

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
