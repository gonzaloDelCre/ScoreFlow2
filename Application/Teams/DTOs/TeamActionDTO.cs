namespace Application.Teams.DTOs
{
    public class TeamActionDTO
    {
        public string Action { get; set; }
        public TeamRequestDTO? Team { get; set; }  // Utilizado para acciones que implican un equipo
        public int? TeamID { get; set; }  // Utilizado para acciones que requieren ID del equipo
    }
}
