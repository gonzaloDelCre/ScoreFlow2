namespace Application.Teams.DTOs
{
    public class TeamRequestDTO
    {
        public int TeamID { get; set; }  // Asegúrate de tener esta propiedad
        public string Name { get; set; }
        public int CoachID { get; set; }  // Asumo que CoachID es de tipo int
        public List<int> PlayerIds { get; set; }
        public string Logo { get; set; }
    }
}

