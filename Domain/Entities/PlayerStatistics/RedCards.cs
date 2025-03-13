namespace Domain.Entities.PlayerStatistics
{
    public class RedCards
    {
        public int Value { get; private set; }

        public RedCards(int value)
        {
            if (value < 0) throw new ArgumentException("Las tarjetas rojas no pueden ser negativas.");
            Value = value;
        }
    }
}