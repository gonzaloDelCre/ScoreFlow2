using Domain.Entities.Notifications;
using Domain.Entities.Users;
using Infrastructure.Persistence.Notifications.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Notifications.Mapper
{
    public class NotificationMapper
    {
        public NotificationEntity MapToEntity(Notification notification)
        {
            return new NotificationEntity
            {
                NotificationID = notification.NotificationID,
                UserID = notification.User.UserID.Value,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }

        public Notification MapToDomain(NotificationEntity entity, User user)
        {
            return new Notification(
                entity.NotificationID,
                user,
                entity.Message,
                entity.Type,
                entity.IsRead,
                entity.CreatedAt
            );
        }
    }
}
