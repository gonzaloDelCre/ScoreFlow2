using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Persistence.Users.Entities;
using Infrastructure.Persistence.Teams.Entities;

namespace Infrastructure.Persistence.Players.Entities
{
    public class Player
    {
        [Key]
        public int PlayerID { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }
        public UserEntity User { get; set; }

        [Required]
        [ForeignKey("Team")]
        public int TeamID { get; set; }
        public Team Team { get; set; }

        public string Position { get; set; }
        public int? Dorsal { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
