using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enum;
using Infrastructure.Persistence.TeamPlayers.Entities;

namespace Infrastructure.Persistence.Players.Entities
{
    [Table("Players")]
    public class PlayerEntity
    {
        [Key]
        public int PlayerID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Position { get; set; } 

        [Required]
        [Range(0, 100)]
        public int Age { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Goals { get; set; }

        [MaxLength(500)]
        public string? Photo { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TeamPlayerEntity> TeamPlayers { get; set; } = new List<TeamPlayerEntity>();
    }
}
