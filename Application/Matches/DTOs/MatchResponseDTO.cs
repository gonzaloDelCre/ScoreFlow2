using Domain.Entities.Teams;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Matches.DTOs
{
    public class MatchResponseDTO
    {
        public MatchID MatchID { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public DateTime MatchDate { get; set; }
        public string Status { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
