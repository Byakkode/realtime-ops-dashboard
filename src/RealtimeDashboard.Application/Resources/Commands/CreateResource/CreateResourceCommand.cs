using MediatR;

namespace RealtimeDashboard.Application.Resources.Commands.CreateResource;

public record CreateResourceCommand(string Name, string Description, string Category, string? Location) : IRequest<Guid>;