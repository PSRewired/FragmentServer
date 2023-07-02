using System.Collections.Generic;

namespace Fragment.NetSlum.Server.Api.Models;

public class PagedResult<T> where T : class
{
    public int Page { get; set; }
    public int TotalRecords { get; set; }
    public int PageSize { get; set; }
    public ICollection<T> Data { get; set; }

    public PagedResult(int page, int pageSize, int totalRecords, ICollection<T> data)
    {
        Page = page;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        Data = data;
    }
}
