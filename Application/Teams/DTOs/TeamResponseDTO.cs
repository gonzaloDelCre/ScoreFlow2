using Domain.Shared;

namespace Application.Teams.DTOs
{
    public class TeamResponseDTO
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }  // <- antes era 'Name'
        public int CoachID { get; set; }
        public List<int> PlayerIds { get; set; }
        public string LogoUrl { get; set; }   // <- antes era 'Logo'
        public DateTime CreatedAt { get; set; }
    }

}
