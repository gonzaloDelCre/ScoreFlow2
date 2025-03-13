namespace Domain.Shared
{
    public class UserID
    {
        public int Value { get; private set; }

        public UserID(int value)
        {
            Value = value;
        }
    }
}
