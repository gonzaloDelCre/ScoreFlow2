using Domain.Enum;
using Domain.Shared;

namespace Application.Playes.DTOs
{
    public class PlayerRequestDTO
    {
        public int? ID { get; set; }
        public string Name { get; set; } = null!;
        public PlayerPosition Position { get; set; }
        public int Age { get; set; }
        public int Goals { get; set; }
        public string? Photo { get; set; }
    }

}
