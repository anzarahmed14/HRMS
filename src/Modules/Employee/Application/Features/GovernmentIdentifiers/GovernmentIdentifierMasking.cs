namespace HRMS.Application.Features.GovernmentIdentifiers;

public static class GovernmentIdentifierMasking
{
    public static string Mask(string identifierNumber)
    {
        if (string.IsNullOrWhiteSpace(identifierNumber))
            return string.Empty;

        if (identifierNumber.Length <= 4)
            return new string('X', identifierNumber.Length);

        return new string('X', identifierNumber.Length - 4)
               + identifierNumber[^4..];
    }
}
