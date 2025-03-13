using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Persistence.Referees.Entities;
using Infrastructure.Persistence.Matches.Entities;

namespace Infrastructure.Persistence.MatchReferees.Entities
{
    public class MatchRefereeEntity
    {
        [Key]
        [Column(Order = 1)]
        public int MatchID { get; set; }
        public MatchEntity Match { get; set; }

        [Key]
        [Column(Order = 2)]
        public int RefereeID { get; set; }
        public RefereeEntity Referee { get; set; }
    }
}
