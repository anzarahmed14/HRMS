namespace HRMS.API.Responses;

public class ErrorResponse
{
    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;


    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    public string? TraceId { get; set; }



    public IDictionary<string, string[]>? Errors { get; set; }
}