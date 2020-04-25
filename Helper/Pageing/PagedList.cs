using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helper.Pageing
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public PagedList(List<T> source, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageIndex;
            TotalPages = count / pageSize;
            if (count % pageSize > 0)
                TotalPages++;
            HasPreviousPage = pageIndex > 1;
            HasNextPage = pageIndex < TotalPages;
            this.AddRange(source);
        }

        public static async Task<PagedList<T>> PagedListAsync(IQueryable<T> source,int pageIndex,int pageSize)
        {
            var count = await source.CountAsync();
            var item=await source.Skip((pageIndex-1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(item, count, pageIndex, pageSize);
        }
    }
}
