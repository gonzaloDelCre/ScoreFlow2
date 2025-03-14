using Domain.Entities.Teams;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Matches.DTOs
{
    public class MatchRequestDTO
    {
        public int MatchID { get; set; }
        public int Team1ID { get; set; } 
        public int Team2ID { get; set; } 
        public DateTime MatchDate { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
    }
}
