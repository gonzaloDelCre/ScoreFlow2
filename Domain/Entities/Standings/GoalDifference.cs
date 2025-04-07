namespace Domain.Entities.Standings
{
    public class GoalDifference
    {
        public int Value { get; private set; }

        public GoalDifference(int value)
        {
            if (value < 0) throw new ArgumentException("La diferencia de goles no puede ser negativa.");
            Value = value;
        }
    }
}
