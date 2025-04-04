﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Enum;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Matches.Entities;

namespace Infrastructure.Persistence.MatchEvents.Entities
{
    public class MatchEventEntity
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [ForeignKey("Match")]
        public int MatchID { get; set; }
        public MatchEntity Match { get; set; }

        [ForeignKey("Player")]
        public int? PlayerID { get; set; }
        public PlayerEntity Player { get; set; }

        [Required]
        public EventType EventType { get; set; }

        [Required]
        public int Minute { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
