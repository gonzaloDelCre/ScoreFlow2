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
        public PlayerID PlayerID { get; set; }
        public string Name { get; set; }
        public TeamID TeamID { get; set; }
        public PlayerPosition Position { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
