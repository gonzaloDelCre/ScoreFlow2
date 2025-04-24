namespace Application.Teams.DTOs
{
    public class TeamResponseDTO
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public List<int> PlayerIds { get; set; } = new List<int>();
        public string LogoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Category { get; set; }
        public string? Stadium { get; set; }
        public string? Club { get; set; }
    }
}
