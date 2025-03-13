
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Persistence.Users.Entities;

namespace Infrastructure.Persistence.Teams.Entities
{
    public class TeamEntity
    {
        [Key]
        public int TeamID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string Logo { get; set; }

        [Required]
        [ForeignKey("User")]
        public int CoachID { get; set; }
        public UserEntity Coach { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
