using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;

namespace Infrastructure.Persistence.Teams.Entities
{
    public class TeamEntity
    {
        [Key]
        public int TeamID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(100)]
        public string? Club { get; set; }

        [MaxLength(100)]
        public string? Stadium { get; set; }

        [MaxLength(500)]
        public string? Logo { get; set; }

        public int? CoachPlayerID { get; set; }

        [ForeignKey("CoachPlayerID")]
        public PlayerEntity? Coach { get; set; }

        public ICollection<TeamPlayerEntity> TeamPlayers { get; set; } = new List<TeamPlayerEntity>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
