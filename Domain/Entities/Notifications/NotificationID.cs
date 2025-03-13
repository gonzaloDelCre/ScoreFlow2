namespace Domain.Entities.Notifications
{
    public class NotificationID
    {
        public int Value { get; private set; }

        public NotificationID(int value)
        {
            if (value <= 0) throw new ArgumentException("El ID de la notificación debe ser mayor que 0.");
            Value = value;
        }
    }
}