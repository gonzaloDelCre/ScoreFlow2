namespace Application.Teams.DTOs
{
    public class TeamRequestDTO
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public List<int> PlayerIds { get; set; }
        public string Logo { get; set; }
    }
}