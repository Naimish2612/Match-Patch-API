using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helper.Pageing
{
    public class PageBaseModel
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;

        //default page size
        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

    }
}
