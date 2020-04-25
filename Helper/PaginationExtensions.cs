using DatingApp.API.Helper.Pageing;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helper
{
    public static class PaginationExtensions
    {
        public static void AddPagination(this HttpResponse resposne, int currentPage, int pageSize, int totalItems, 
            int totalPages, bool hasPreviousPage, bool hasNextPage)
        {
            var paginationHeader = new PaginationHeader(currentPage, pageSize, totalItems, totalPages,hasPreviousPage,hasNextPage);

            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();

            resposne.Headers.Add("Pagination",JsonConvert.SerializeObject(paginationHeader,camelCaseFormatter));
            resposne.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
