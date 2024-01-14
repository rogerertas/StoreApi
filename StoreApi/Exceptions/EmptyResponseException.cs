namespace StoreApi.Exceptions
{
        public class EmptyResponseException : Exception
        {
            public EmptyResponseException(string message) : base(message) { }
        }
    }
