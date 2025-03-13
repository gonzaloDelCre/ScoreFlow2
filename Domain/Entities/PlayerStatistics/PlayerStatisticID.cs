namespace Domain.Entities.PlayerStatistics
{
    public class PlayerStatisticID
    {
        public int Value { get; private set; }

        public PlayerStatisticID(int value)
        {
            if (value <= 0) throw new ArgumentException("El ID de la estadística del jugador debe ser mayor que 0.");
            Value = value;
        }
    }
}