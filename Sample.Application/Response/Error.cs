using System.Collections.Generic;

namespace Sample.Application.Response
{
    public class Error
    {
        /// <summary>
        /// Class that thrown the error
        /// </summary>
        public string Model { get; protected set; }
        /// <summary>
        /// Method that thrown the error
        /// </summary>
        public string Reference { get; private set; }
        /// <summary>
        /// Error Message
        /// </summary>
        public IEnumerable<string> Messages { get; private set; }

        /// <summary>
        /// Initializes an error instance with a model, reference and message
        /// </summary>
        /// <param name="model">Class that thrown the error</param>
        /// <param name="reference">Method that thrown the error</param>
        /// <param name="messages">Error Messages</param>
        public Error(string model, string reference, IEnumerable<string> messages)
        {
            Model = model;
            Reference = reference;
            Messages = messages;
        }
    }
}