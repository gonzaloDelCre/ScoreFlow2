using Domain.Enum;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.DTOs
{
    public class PlayerStatisticResponseDTO
    {
        public int PlayerStatisticID { get; set; }
        public int MatchID { get; set; }
        public int PlayerID { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int? MinutesPlayed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
