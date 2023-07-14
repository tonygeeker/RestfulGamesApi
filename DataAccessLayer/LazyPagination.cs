using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using System.Collections;

namespace RestfulGamesApi.DataAccessLayer
{
    public class LazyPagination<T> : IPagination<T>
    {
        /// <summary>
        /// Default page size.
        /// </summary>
        public const int DefaultPageSize = 10;
        public IList<T> Results;
        private int totalItems;
        public int PageSize { get; private set; }
        /// <summary>
        /// The query to execute.
        /// </summary>
        public IQueryable<T> Query { get; private set; }
        public int PageNumber { get; private set; }
        public bool IncludePrev { get; private set; }


        /// <summary>
        /// Creates a new instance of the <see cref="LazyPagination{T}"/> class.
        /// </summary>
        /// <param name="query">The query to page.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">Number of items per page.</param>
        public LazyPagination(IQueryable<T> query, int pageNumber, int pageSize, bool includePrev = false)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Query = query;
            IncludePrev = includePrev;

        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            TryExecuteQuery();

            foreach (var item in Results)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Executes the query if it has not already been executed.
        /// </summary>
        protected void TryExecuteQuery()
        {
            //Results is not null, means query has already been executed.
            if (Results != null)
                return;

            totalItems = Query.Count();
            Results = ExecuteQuery();
        }

        /// <summary>
        /// Calls Queryable.Skip/Take to perform the pagination.
        /// </summary>
        /// <returns>The paged set of results.</returns>
        protected virtual IList<T> ExecuteQuery()
        {
            if (IncludePrev)
            {
                return Query.Take(PageSize * PageNumber).ToList();
            }
            else
            {
                int numberToSkip = (PageNumber - 1) * PageSize;
                return Query.Skip(numberToSkip).Take(PageSize).ToList();
            }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        public int TotalItems
        {
            get
            {
                TryExecuteQuery();
                return totalItems;
            }
        }

        public int TotalPages
        {
            get { return (int)Math.Ceiling(((double)TotalItems) / PageSize); }
        }

        public int FirstItem
        {
            get
            {
                TryExecuteQuery();
                return ((PageNumber - 1) * PageSize) + 1;
            }
        }

        public int LastItem
        {
            get
            {
                return FirstItem + Results.Count - 1;
            }
        }

        public bool HasPreviousPage
        {
            get { return PageNumber > 1; }
        }

        public bool HasNextPage
        {
            get { return PageNumber < TotalPages; }
        }
    }
}
