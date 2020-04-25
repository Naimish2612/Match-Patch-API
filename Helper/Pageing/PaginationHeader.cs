using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helper.Pageing
{
    public class PaginationHeader
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public PaginationHeader(int currentPage,int pageSize,int totalItems,int totalPages,bool hasPreviousPage, bool hasNextPage)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;
            this.HasPreviousPage = hasPreviousPage;
            this.HasNextPage = hasNextPage;
        }
    }
}
