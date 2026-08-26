namespace HRMS.Application.Features.BankAccounts;

public static class BankAccountMasking
{
    public static string MaskAccountNumber(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            return string.Empty;

        if (accountNumber.Length <= 4)
            return new string('X', accountNumber.Length);

        return new string('X', accountNumber.Length - 4)
               + accountNumber[^4..];
    }
}
