namespace Domain.Entities.Standings
{
    public class Wins
    {
        public int Value { get; private set; }

        public Wins(int value)
        {
            if (value < 0) throw new ArgumentException("El número de victorias no puede ser negativo.");
            Value = value;
        }
    }
}
