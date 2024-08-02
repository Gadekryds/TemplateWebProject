namespace TemplateWebProject.Errors;

public class ServiceException(int statusCode, string message) : Exception
{
    public int StatusCode { get; } = statusCode;
    public string ExceptionMessage { get; } = message;
}
