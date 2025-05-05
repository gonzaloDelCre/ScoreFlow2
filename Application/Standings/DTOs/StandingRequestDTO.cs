namespace Application.Standings.DTOs
{
    public class StandingRequestDTO
    {
        public int StandingID { get; set; }
        public int LeagueID { get; set; } 
        public int TeamID { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
    }
}
