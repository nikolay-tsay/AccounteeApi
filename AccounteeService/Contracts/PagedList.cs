﻿namespace AccounteeService.Contracts;

public sealed class PagedList<T>
{
    public int PageNum  { get; set; }
    public int PageSize   { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<T>? Items { get; set; }
}