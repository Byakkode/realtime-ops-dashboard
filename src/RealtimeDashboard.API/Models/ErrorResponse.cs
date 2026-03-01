namespace RealtimeDashboard.API.Models;

public record ErrorResponse(
    string Type,
    string Title,
    int Status,
    string Detail,
    IReadOnlyDictionary<string, string[]>? Errors = null
);