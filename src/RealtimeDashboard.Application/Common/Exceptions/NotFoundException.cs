namespace RealtimeDashboard.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key) : base($"{entityName} with id '{key}' wad not found.")
    {
        
    }
}