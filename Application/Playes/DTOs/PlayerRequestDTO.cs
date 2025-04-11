using Domain.Enum;
using Domain.Shared;

namespace Application.Playes.DTOs
{
    public class PlayerRequestDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public PlayerPosition Position { get; set; }
        public int Goals { get; set; }
        public string? Photo { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<int> TeamIDs { get; set; }  
    }

}
