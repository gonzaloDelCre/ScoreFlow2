namespace Domain.Shared
{
    public class PlayerID
    {
        public int Value { get; private set; }

        public PlayerID(int value)
        {
            if (value <= 0) throw new ArgumentException("El ID del jugador debe ser mayor que 0.");
            Value = value;
        }
    }
}