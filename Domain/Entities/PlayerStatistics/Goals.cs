namespace Domain.Entities.PlayerStatistics
{
    public class Goals
    {
        public int Value { get; private set; }

        public Goals(int value)
        {
            if (value < 0) throw new ArgumentException("Los goles no pueden ser negativos.");
            Value = value;
        }
    }
}