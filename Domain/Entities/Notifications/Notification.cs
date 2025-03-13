using Domain.Entities.Users;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Entities.Notifications
{
    public class Notification
    {
        public NotificationID NotificationID { get; private set; }
        public UserID UserID { get; private set; }
        public NotificationMessage Message { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User User { get; private set; }

        public Notification(NotificationID notificationID, UserID userID, NotificationMessage message, NotificationType type, User user, DateTime createdAt)
        {
            NotificationID = notificationID ?? throw new ArgumentNullException(nameof(notificationID));
            UserID = userID ?? throw new ArgumentNullException(nameof(userID));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Type = type;
            User = user ?? throw new ArgumentNullException(nameof(user));
            CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        }


    }
}
