using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MatchEvents.DTOs
{
    public class MatchEventResponseDTO
    {
        public int ID { get; set; }
        public int MatchID { get; set; }
        public int? PlayerID { get; set; }
        public string? PlayerName { get; set; }
        public EventType EventType { get; set; }
        public int Minute { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
