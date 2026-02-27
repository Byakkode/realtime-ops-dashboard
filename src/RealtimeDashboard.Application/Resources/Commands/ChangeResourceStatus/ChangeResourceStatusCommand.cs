using MediatR;
using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.Application.Resources.Commands.ChangeResourceStatus;

public record ChangeResourceStatusCommand(Guid ResourceId, ResourceStatus NewStatus, string ChangedBy) : IRequest;