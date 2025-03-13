namespace Domain.Entities.MatchEvents
{
    public class EventType
    {
        public string Value { get; private set; }

        public EventType(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("El tipo de evento no puede estar vacío.");
            if (value.Length > 100)
                throw new ArgumentException("El tipo de evento es demasiado largo.");
            Value = value;
        }
    }
}
