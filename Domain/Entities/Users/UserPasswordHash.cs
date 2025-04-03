namespace Domain.Entities.Users
{
    public class UserPasswordHash
    {
        public string? Value { get; private set; }
        
        public UserPasswordHash(string? value)
        {
            Value = value;
        }
    }
}