namespace Application.Teams.DTOs
{
    public class TeamActionDTO
    {
        public string Action { get; set; }
        public TeamRequestDTO? Team { get; set; }  
        public int? TeamID { get; set; }  
    }
}
