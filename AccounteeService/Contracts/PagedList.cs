namespace AccounteeService.Contracts;

public class PagedList<T>
{
    public int PageIndex  { get; set; }
    public int PageSize   { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<T>? Items { get; set; }
    
    public bool HasPreviousPage => PageIndex > 0;
    public bool HasNextPage => PageIndex + 1 < TotalPages;
}