using Domain.Shared;

namespace Application.Teams.DTOs
{
    public class TeamResponseDTO
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string LogoUrl { get; set; }
        public string CoachName { get; set; }
        public int CoachID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
