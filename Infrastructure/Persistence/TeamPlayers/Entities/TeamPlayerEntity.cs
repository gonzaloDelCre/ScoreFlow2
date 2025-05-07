using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enum;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Teams.Entities;

namespace Infrastructure.Persistence.TeamPlayers.Entities
{
    [Table("TeamPlayers")]
    public class TeamPlayerEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int TeamID { get; set; }

        [ForeignKey(nameof(TeamID))]
        public TeamEntity Team { get; set; }

        [Required]
        public int PlayerID { get; set; }

        [ForeignKey(nameof(PlayerID))]
        public PlayerEntity Player { get; set; }

        [Required]
        public RoleInTeam RoleInTeam { get; set; }

        [Required]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
