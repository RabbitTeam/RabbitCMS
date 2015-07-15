using System;
using System.Linq;

namespace Rabbit.Infrastructures.Data
{
    public class PageParameter
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public PageParameter(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int PageCount { get; set; }

        public IQueryable<T> Paged<T>(IQueryable<T> queryable)
        {
            var skip = PageIndex * PageSize;
            PageCount = (int)Math.Ceiling(queryable.Count() / (double)PageSize);
            return queryable.Skip(skip).Take(PageSize);
        }
    }
}