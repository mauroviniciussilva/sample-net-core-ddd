using System.Collections.Generic;

namespace Sample.Domain.Entities
{
    public class QueryFilter
    {
        public Dictionary<string, string> Filters { get; set; }
        public int Start { get; set; }
        public int Limit { get; set; }

        #region [ Constructor ]

        public QueryFilter()
        {
            Filters = new Dictionary<string, string>();
        }

        #endregion

        #region [ Methods ]

        public void AddFilter(string property, string value)
        {
            if (Filters == null)
            {
                Filters = new Dictionary<string, string>();
            }

            Filters.Add(property, value);
        }

        #endregion
    }
}