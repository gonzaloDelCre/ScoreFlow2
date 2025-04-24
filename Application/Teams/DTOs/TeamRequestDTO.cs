namespace Application.Teams.DTOs
{
    public class TeamRequestDTO
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public List<int> PlayerIds { get; set; } = new List<int>();
        public int TeamID { get; set; }
        public string? Category { get; set; }
        public string? Stadium { get; set; }
        public string? Club { get; set; }
    }
}
