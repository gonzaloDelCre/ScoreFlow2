using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Persistence.Referees.Entities;
using Infrastructure.Persistence.Matches.Entities;

namespace Infrastructure.Persistence.MatchReferees.Entities
{
    public class MatchReferee
    {
        [Key]
        [Column(Order = 1)]
        public int MatchID { get; set; }
        public Match Match { get; set; }

        [Key]
        [Column(Order = 2)]
        public int RefereeID { get; set; }
        public Referee Referee { get; set; }
    }
}
