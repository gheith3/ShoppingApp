namespace ShoppingApp.Core.Common.Models;

public class ApiException : Exception
{
    public ApiException()
    {
    }

    public ApiException(string message, int code, string errorTitle = "_") : base(message)
    {
        ErrorTitle = errorTitle;
        ErrorCode = code;
    }

    public string ErrorTitle { get; set; }
    public int ErrorCode { get; set; }
}