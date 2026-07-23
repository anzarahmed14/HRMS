namespace HRMS.Shared.Models;

public class PagedRequest
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; }

    public bool Descending { get; set; } = false;
}