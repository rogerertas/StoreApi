namespace StoreApi.Exceptions
{
    public class InvalidJsonResponseException : Exception
    {
        public InvalidJsonResponseException(string message) : base(message) { }
    }
}
