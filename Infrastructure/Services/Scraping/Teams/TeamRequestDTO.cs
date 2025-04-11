using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Teams
{
    public class TeamRequestDTO
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public List<int> PlayerIds { get; set; }
        public string Logo { get; set; }
    }

}
