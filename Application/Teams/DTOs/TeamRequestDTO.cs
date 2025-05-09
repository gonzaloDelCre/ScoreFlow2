namespace Application.Teams.DTOs
{
    public class TeamRequestDTO
    {
        public int? ID { get; set; }
        public string? ExternalID { get; set; }
        public string Name { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public string? Category { get; set; }
        public string? Club { get; set; }
        public string? Stadium { get; set; }
        public int? CoachPlayerID { get; set; }
    }
}
