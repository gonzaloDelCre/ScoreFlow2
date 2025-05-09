using Application.PlayerStatistics.DTOs;
using Application.TeamPlayers.DTOs;
using Domain.Enum;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.DTOs
{
    public class PlayerResponseDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public PlayerPosition Position { get; set; }
        public int Age { get; set; }
        public int Goals { get; set; }
        public string? Photo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

