using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.Application.Resources.Queries.GetAllResources;

public record ResourceDto(
    Guid Id,
    string Name,
    string Description,
    string Category,
    string? Location,
    ResourceStatus? Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);