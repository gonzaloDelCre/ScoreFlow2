using Domain.Shared;

namespace Application.Teams.DTOs
{
    public class TeamResponseDTO
    {
        public int TeamID { get; set; }
        public string Name { get; set; }  
        public int CoachID { get; set; }  
        public List<int> PlayerIds { get; set; }
        public string Logo { get; set; }
        public DateTime CreatedAt { get; set; }  
    }
}
