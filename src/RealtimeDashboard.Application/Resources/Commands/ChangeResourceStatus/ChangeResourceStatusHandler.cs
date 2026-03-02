using MediatR;
using RealtimeDashboard.Application.Common.Exceptions;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Application.Resources.Commands.ChangeResourceStatus;

public class ChangeResourceStatusHandler : IRequestHandler<ChangeResourceStatusCommand>
{
    private readonly IResourceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public ChangeResourceStatusHandler(
        IResourceRepository repository,
        IUnitOfWork unitOfWork,
        IPublisher publisher)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task Handle(
        ChangeResourceStatusCommand request,
        CancellationToken cancellationToken)
    {
        var resource = await _repository.GetByIdAsync(request.ResourceId, cancellationToken)
                       ?? throw new NotFoundException(nameof(Resource), request.ResourceId);

        resource.ChangeStatus(request.NewStatus, request.ChangedBy);
        _repository.Update(resource);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in resource.DomainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        resource.ClearDomainEvents();
    }
}