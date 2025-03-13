
using Domain.Entities.Matches;
using Domain.Entities.Referees;

namespace Domain.Entities.MatchReferees
{
    public class MatchReferee
    {
        public int MatchID { get; set; }
        public int RefereeID { get; set; }

        public Match Match { get; set; }
        public Referee Referee { get; set; }
    }

}
