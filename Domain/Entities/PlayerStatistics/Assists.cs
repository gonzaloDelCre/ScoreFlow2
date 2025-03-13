namespace Domain.Entities.PlayerStatistics
{
    public class Assists
    {
        public int Value { get; private set; }

        public Assists(int value)
        {
            if (value < 0) throw new ArgumentException("Las asistencias no pueden ser negativas.");
            Value = value;
        }
    }
}