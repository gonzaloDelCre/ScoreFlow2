using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Enum;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Matches.Entities;

namespace Infrastructure.Persistence.MatchEvents.Entities
{
    [Table("MatchEvents")]
    public class MatchEventEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int MatchID { get; set; }

        [ForeignKey(nameof(MatchID))]
        public MatchEntity Match { get; set; }

        public int? PlayerID { get; set; }

        [ForeignKey(nameof(PlayerID))]
        public PlayerEntity Player { get; set; }

        [Required]
        public EventType EventType { get; set; }

        [Required]
        public int Minute { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
