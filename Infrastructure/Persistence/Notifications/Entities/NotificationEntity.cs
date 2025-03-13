using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Enum;
using Infrastructure.Persistence.Users.Entities;

namespace Infrastructure.Persistence.Notifications.Entities
{
    public class NotificationEntity
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }
        public UserEntity User { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
