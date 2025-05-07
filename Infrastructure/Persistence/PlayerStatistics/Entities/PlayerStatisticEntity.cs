using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Matches.Entities;

namespace Infrastructure.Persistence.PlayerStatistics.Entities
{
    [Table("PlayerStatistics")]
    public class PlayerStatisticEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int PlayerID { get; set; }

        [ForeignKey(nameof(PlayerID))]
        public PlayerEntity Player { get; set; }

        [Required]
        public int MatchID { get; set; }

        [ForeignKey(nameof(MatchID))]
        public MatchEntity Match { get; set; }

        [Required]
        public int Goals { get; set; } = 0;

        [Required]
        public int Assists { get; set; } = 0;

        [Required]
        public int YellowCards { get; set; } = 0;

        [Required]
        public int RedCards { get; set; } = 0;

        public int? MinutesPlayed { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
