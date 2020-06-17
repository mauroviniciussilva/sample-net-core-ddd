using Sample.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Sample.Domain.Interfaces.Repositories
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }

    public class PagedResult<T> : PagedResultBase where T : EntityBase
    {
        public IList<T> Result { get; set; }

        public PagedResult()
        {
            Result = new List<T>();
        }
    }
}