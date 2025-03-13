namespace Domain.Entities.Standings
{
    public class Points
    {
        public int Value { get; private set; }

        public Points(int value)
        {
            if (value < 0) throw new ArgumentException("Los puntos no pueden ser negativos.");
            Value = value;
        }
    }
}