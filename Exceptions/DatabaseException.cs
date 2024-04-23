namespace PokemonAPIusingDapper.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {
        }

        public DatabaseException(string message, Exception cause) : base(message, cause)
        {
        }
    }
}