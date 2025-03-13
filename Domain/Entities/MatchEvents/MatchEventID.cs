namespace Domain.Entities.MatchEvents
{
    public class MatchEventID
    {
        public int Value { get; private set; }

        public MatchEventID(int value)
        {
            if (value <= 0) throw new ArgumentException("El ID del evento debe ser mayor que 0.");
            Value = value;
        }
    }
}