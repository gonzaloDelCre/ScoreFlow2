namespace Domain.Entities.Standings
{
    public class MatchesPlayed
    {
        public int Value { get; private set; }

        public MatchesPlayed(int value)
        {
            if (value < 0) throw new ArgumentException("El número de partidos jugados no puede ser negativo.");
            Value = value;
        }
    }
}
