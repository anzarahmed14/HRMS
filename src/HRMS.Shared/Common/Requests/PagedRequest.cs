using HRMS.Shared.Common.Constants;

namespace HRMS.Shared.Common.Requests;

public class PagedRequest
{
    private int _pageSize = ApplicationConstants.DefaultPageSize;

    public int PageNumber { get; set; } = ApplicationConstants.DefaultPageNumber;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > ApplicationConstants.MaxPageSize
            ? ApplicationConstants.MaxPageSize
            : value;
    }
}