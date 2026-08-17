namespace HRMS.BuildingBlocks.Application.Pagination;

public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];

    public int TotalRecords { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalPages =>
        PageSize == 0
            ? 0
            : (int)Math.Ceiling((double)TotalRecords / PageSize);
}