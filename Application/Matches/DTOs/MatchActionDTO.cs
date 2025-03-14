using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Matches.DTOs
{
    public class MatchActionDTO
    {
        public string Action { get; set; }
        public MatchID? MatchID { get; set; }
        public MatchRequestDTO? Match { get; set; }
    }

}
