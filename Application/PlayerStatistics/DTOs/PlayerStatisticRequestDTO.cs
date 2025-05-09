using Domain.Enum;
using Domain.Shared;

namespace Application.PlayerStatistics.DTOs
{
    public class PlayerStatisticRequestDTO
    {
        public int? ID { get; set; }
        public int MatchID { get; set; }
        public int PlayerID { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int? MinutesPlayed { get; set; }
    }
}