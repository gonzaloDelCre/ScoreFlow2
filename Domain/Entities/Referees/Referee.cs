using Domain.Entities.MatchReferees;
using Domain.Shared;

namespace Domain.Entities.Referees
{
    public class Referee
    {
        public RefereeID RefereeID { get; private set; }
        public RefereeName Name { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<MatchReferee> MatchReferees { get; private set; }

        public Referee(RefereeID refereeID, RefereeName name, DateTime createdAt)
        {
            RefereeID = refereeID ?? throw new ArgumentNullException(nameof(refereeID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
            MatchReferees = new List<MatchReferee>();
        }
        public Referee() => MatchReferees = new List<MatchReferee>();

    }

}
