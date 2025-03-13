﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Persistence.Teams.Entities;
using Infrastructure.Persistence.Leagues.Entities;

namespace Infrastructure.Persistence.TeamLeagues.Entities
{
    public class TeamLeague
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Team")]
        public int TeamID { get; set; }
        public Team Team { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("League")]
        public int LeagueID { get; set; }
        public League League { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
    }
}
