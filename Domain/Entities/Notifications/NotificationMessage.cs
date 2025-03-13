namespace Domain.Entities.Notifications
{
    public class NotificationMessage
    {
        public string Value { get; private set; }

        public NotificationMessage(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("El mensaje de la notificación no puede estar vacío.");
            if (value.Length > 500) 
                throw new ArgumentException("El mensaje de la notificación es demasiado largo.");
            Value = value;
        }
    }
}