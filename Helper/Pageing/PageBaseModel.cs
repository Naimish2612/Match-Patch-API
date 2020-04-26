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

        public int user_id { get; set; }
        public string gender { get; set; }

        public int min_age { get; set; } = 18;
        public int max_age { get; set; } = 99;

    }
}
