namespace HRMS.BuildingBlocks.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string entityName, object key)
        : base($"{entityName} with Id '{key}' was not found.")
    {
    }
}