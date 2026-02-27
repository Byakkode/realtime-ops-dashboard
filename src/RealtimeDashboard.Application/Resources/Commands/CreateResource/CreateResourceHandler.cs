using MediatR;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Application.Resources.Commands.CreateResource;

public class CreateResourceHandler : IRequestHandler<CreateResourceCommand, Guid>
{
    private readonly IResourceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateResourceHandler(
        IResourceRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateResourceCommand request,
        CancellationToken cancellationToken)
    {
        var resource = Resource.Create(
            request.Name,
            request.Description,
            request.Category,
            request.Location
        );

        await _repository.AddAsync(resource, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return resource.Id;
    }
}