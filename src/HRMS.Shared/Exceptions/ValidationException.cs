using HRMS.Shared.Responses;

namespace HRMS.Shared.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
    {
        Errors = new List<ApiError>();
    }

    public ValidationException(string message)
        : base(message)
    {
        Errors = new List<ApiError>();
    }

    public ValidationException(List<ApiError> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public List<ApiError> Errors { get; }
}