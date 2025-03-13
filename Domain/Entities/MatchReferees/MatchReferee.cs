using Domain.Entities.Matches;
using Domain.Entities.Referees;
using Domain.Shared;

namespace Domain.Entities.MatchReferees
{
    public class MatchReferee
    {
        public MatchID MatchID { get; private set; }
        public RefereeID RefereeID { get; private set; }

        public Match Match { get; private set; }
        public Referee Referee { get; private set; }

        public MatchReferee(MatchID matchID, RefereeID refereeID, Match match, Referee referee)
        {
            MatchID = matchID ?? throw new ArgumentNullException(nameof(matchID));
            RefereeID = refereeID ?? throw new ArgumentNullException(nameof(refereeID));
            Match = match ?? throw new ArgumentNullException(nameof(match));
            Referee = referee ?? throw new ArgumentNullException(nameof(referee));
        }
    }

}
