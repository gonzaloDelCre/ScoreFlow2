namespace Domain.Shared
{
    public class RefereeID
    {
        public int Value { get; private set; }

        public RefereeID(int value)
        {
            if (value <= 0) throw new ArgumentException("El ID del árbitro debe ser mayor que 0.");
            Value = value;
        }
    }
}