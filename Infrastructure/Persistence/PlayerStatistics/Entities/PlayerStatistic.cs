using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Matches.Entities;

namespace Infrastructure.Persistence.PlayerStatistics.Entities
{
    public class PlayerStatistic
    {
        [Key]
        public int StatID { get; set; }

        [Required]
        [ForeignKey("Player")]
        public int PlayerID { get; set; }
        public Player Player { get; set; }

        [Required]
        [ForeignKey("Match")]
        public int MatchID { get; set; }
        public Match Match { get; set; }

        public int Goals { get; set; } = 0;
        public int Assists { get; set; } = 0;
        public int YellowCards { get; set; } = 0;
        public int RedCards { get; set; } = 0;
        public int? MinutesPlayed { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
