namespace Domain.Entities.Players
{
    public class PlayerAge
    {
        public int Value { get; private set; }

        public PlayerAge(int value)
        {
            if (value <= 0)
                throw new ArgumentException("La edad del jugador debe ser mayor que cero.");
            Value = value;
        }

        public static implicit operator int(PlayerAge age) => age.Value;
        public static implicit operator PlayerAge(int value) => new PlayerAge(value);
    }
}
