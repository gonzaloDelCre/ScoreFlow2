using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Users.Entities
{
    public class UserEntity
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
