namespace DoeAgasalhoApiV2._0.Exceptions
{
    public class BusinessOperationException : Exception
    {
        public BusinessOperationException() : base()
        {
        }

        public BusinessOperationException(string message) : base(message)
        {
        }

        public BusinessOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
