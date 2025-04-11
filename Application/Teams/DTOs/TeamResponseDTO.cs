namespace Application.Teams.DTOs
{
    public class TeamResponseDTO
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public List<int> PlayerIds { get; set; }
        public string LogoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}