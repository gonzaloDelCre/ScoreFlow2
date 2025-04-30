using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Persistence.Standings.Entities;
using Infrastructure.Persistence.Teams.Entities;

namespace Infrastructure.Persistence.Leagues.Entities
{
    public class LeagueEntity
    {
        [Key]
        public int LeagueID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relaciones
        public ICollection<StandingEntity> Standings { get; set; } = new List<StandingEntity>();

        // ← colección directa
        public ICollection<TeamEntity> Teams { get; set; } = new List<TeamEntity>();
    }
}
