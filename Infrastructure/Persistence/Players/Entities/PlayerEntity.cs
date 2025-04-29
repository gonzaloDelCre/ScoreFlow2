using System.ComponentModel.DataAnnotations;
using Domain.Enum;
using Infrastructure.Persistence.TeamPlayers.Entities;

namespace Infrastructure.Persistence.Players.Entities
{
    public class PlayerEntity
    {
        [Key]
        public int PlayerID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; } // Convertimos en string para la persistencia, ya que PlayerPosition es un enum

        [Range(0, 100)]
        public int Age { get; set; }

        [Range(0, int.MaxValue)]
        public int Goals { get; set; }

        [MaxLength(500)]
        public string? Photo { get; set; }

        public DateTime CreatedAt { get; set; } =DateTime.UtcNow;

        // Relación muchos a muchos con equipos
        public ICollection<TeamPlayerEntity> TeamPlayers { get; set; } = new List<TeamPlayerEntity>();
    }
}
