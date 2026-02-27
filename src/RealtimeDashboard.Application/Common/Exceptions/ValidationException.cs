namespace RealtimeDashboard.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(string field, string message) : base("Validation failed.")
    {
        Errors = new Dictionary<string, string[]>
        {
            { field, [message] }
        };
    }
}