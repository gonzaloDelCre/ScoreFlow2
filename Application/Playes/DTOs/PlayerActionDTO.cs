using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.DTOs
{
    public class PlayerActionDTO
    {
        public string Action { get; set; }
        public PlayerRequestDTO? Player { get; set; }
        public PlayerID? PlayerID { get; set; }
    }
}
