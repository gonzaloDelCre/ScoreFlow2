using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.DTOs
{
    public class PlayerStatisticActionDTO
    {
        public string Action { get; set; }
        public int PlayerStatisticID { get; set; }
        public PlayerStatisticRequestDTO PlayerStatistic { get; set; }
    }
}
