namespace Domain.Entities.PlayerStatistics
{
    public class YellowCards
    {
        public int Value { get; private set; }

        public YellowCards(int value)
        {
            if (value < 0) throw new ArgumentException("Las tarjetas amarillas no pueden ser negativas.");
            Value = value;
        }
    }
}