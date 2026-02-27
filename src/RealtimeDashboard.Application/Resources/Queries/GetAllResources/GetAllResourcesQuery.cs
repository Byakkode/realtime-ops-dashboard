using MediatR;

namespace RealtimeDashboard.Application.Resources.Queries.GetAllResources;

public record GetAllResourcesQuery(string? Category = null) : IRequest<IReadOnlyList<ResourceDto>>;