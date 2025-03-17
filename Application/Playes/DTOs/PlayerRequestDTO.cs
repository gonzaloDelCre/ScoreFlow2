using Domain.Enum;
using Domain.Shared;

namespace Application.Playes.DTOs
{
    public class PlayerRequestDTO
    {
        public string Name { get; set; }
        public TeamID TeamID { get; set; }
        public PlayerPosition Position { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}