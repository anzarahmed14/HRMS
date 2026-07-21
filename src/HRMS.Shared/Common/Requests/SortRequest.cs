using HRMS.Shared.Common.Enums;

namespace HRMS.Shared.Common.Requests;

public class SortRequest
{
    public string? SortBy { get; set; }

    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}