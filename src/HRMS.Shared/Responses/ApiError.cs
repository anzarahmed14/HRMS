namespace HRMS.Shared.Responses;

public class ApiError
{
    public string? Field { get; set; }

    public string Message { get; set; } = string.Empty;
}