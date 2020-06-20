using System.Collections.Generic;

namespace Sample.Application.Response
{
    public class Result
    {
        /// <summary>
        /// An Empty success result
        /// </summary>
        public static Result Empty = new Result();

        private readonly List<Error> _errors = new List<Error>();

        /// <summary>
        /// Indicates if there's an error into Result 
        /// </summary>
        public bool HasErrors => _errors.Count > 0;

        /// <summary>
        /// Errors List
        /// </summary>
        public List<Error> Errors => _errors;

        /// <summary>
        /// Protected Contructor to Empty result
        /// </summary>
        protected Result() { }

        /// <summary>
        /// Initializes an instace of Result with an Error
        /// </summary>
        /// <param name="error"></param>
        public Result(Error error)
        {
            AddError(error);
        }

        /// <summary>
        /// Initializes an instance of Result with a list of Errors
        /// </summary>
        /// <param name="errors"></param>
        public Result(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
                AddError(error);
        }

        /// <summary>
        /// Add an Error to List
        /// </summary>
        /// <param name="error">Error</param>
        public void AddError(Error error)
        {
            Errors.Add(error);
        }
    }

    /// <summary>
    /// Represents a result with a generic data
    /// </summary>
    /// <typeparam name="TData">Generic Data</typeparam>
    public class Result<TData>
    {
        /// <summary>
        /// Generic Data
        /// </summary>
        public TData Data { get; }
        /// <summary>
        /// Count Data
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Initializes an instance of Result with a generic Data
        /// </summary>
        /// <param name="data"></param>
        public Result(TData data)
        {
            Data = data;
        }

        /// <summary>
        /// Initializes an instance of Result with a generic Data with total
        /// </summary>
        /// <param name="data"></param>
        /// <param name="total"></param>
        public Result(TData data, int total)
        {
            Data = data;
            Total = total;
        }
    }
}
