using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatApp.Application.Helpers;
public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response,int currentPage,int itemsPerPage,int totalItems,int totalPages)
    {
        PaginationHeader paginationHeader = new(currentPage,itemsPerPage,totalItems,totalPages);
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

    }
}
