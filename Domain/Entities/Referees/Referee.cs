using Domain.Entities.MatchReferees;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Referees
{
    public class Referee
    {
        public int RefereeID { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MatchReferee> MatchReferees { get; set; } = new List<MatchReferee>();
    }

}
