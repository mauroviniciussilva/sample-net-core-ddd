namespace Sample.Application.Response
{
    public class Error
    {
        /// <summary>
        /// Error Code
        /// </summary>
        public string Reference { get; private set; }
        /// <summary>
        /// Error Message
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Initializes an error instance with a code and message
        /// </summary>
        /// <param name="reference">Error Reference</param>
        /// <param name="message">Error Message</param>
        public Error(string reference, string message)
        {
            SetReference(reference);
            SetMessage(message);
        }

        /// <summary>
        /// Initializes an error instance with a message
        /// </summary>
        /// <param name="message">Error Message</param>
        public Error(string message)
        {
            SetMessage(message);
        }

        /// <summary>
        /// Set the Code
        /// </summary>
        /// <param name="reference">Code</param>
        private void SetReference(string reference)
        {
            Reference = reference;
        }

        /// <summary>
        /// Set the Message
        /// </summary>
        /// <param name="message">Message</param>
        private void SetMessage(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Returns the error message
        /// </summary>
        /// <returns>Error Message</returns>
        public override string ToString() => Message;
    }
}
