namespace Domain.Shared
{
    public class MatchID
    {
        public int Value { get; private set; }

        public MatchID(int value)
        {
            if (value <= 0) throw new ArgumentException("El ID del partido debe ser mayor que 0.");
            Value = value;
        }
    }
}