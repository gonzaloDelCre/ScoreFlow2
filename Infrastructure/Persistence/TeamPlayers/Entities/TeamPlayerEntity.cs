using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enum;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Teams.Entities;

namespace Infrastructure.Persistence.TeamPlayers.Entities
{
    public class TeamPlayerEntity
    {
        [Key]
        public int Id { get; set; }

        public int TeamID { get; set; }
        [ForeignKey("TeamID")]
        public TeamEntity Team { get; set; }

        public int PlayerID { get; set; }
        [ForeignKey("PlayerID")]
        public PlayerEntity Player { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public RoleInTeam RoleInTeam { get; set; } 

    }
}
