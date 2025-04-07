namespace Domain.Entities.Standings
{
    public class Losses
    {
        public int Value { get; private set; }

        public Losses(int value)
        {
            if (value < 0) throw new ArgumentException("El número de derrotas no puede ser negativo.");
            Value = value;
        }
    }
}
