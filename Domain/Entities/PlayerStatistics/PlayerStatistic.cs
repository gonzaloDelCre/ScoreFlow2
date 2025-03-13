using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities.Players;
using Domain.Entities.Matches;

namespace Domain.Entities.PlayerStatistics
{
    public class PlayerStatistic
    {
        public int PlayerStatisticID { get; set; }
        public int MatchID { get; set; }
        public int PlayerID { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Match Match { get; set; }
        public Player Player { get; set; }
    }

}
