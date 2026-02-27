using MediatR;
using RealtimeDashboard.Application.Resources.Queries.GetAllResources;

namespace RealtimeDashboard.Application.Resources.Queries.GetResourceById;

public record GetResourceByIdQuery(Guid Id) : IRequest<ResourceDto>;