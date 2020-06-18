namespace Sample.Domain.Exceptions
{
    public class ExceptionItemInfo
    {
        public string Model { get; protected set; }
        public string Reference { get; protected set; }
        public string Message { get; protected set; }
        public object[] Arguments { get; protected set; }

        public ExceptionItemInfo(string model, string reference, string message, params object[] arguments)
        {
            Model = model;
            Reference = reference;
            Message = message;
            Arguments = arguments;
        }
    }
}