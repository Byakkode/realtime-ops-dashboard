using MediatR;
using RealtimeDashboard.Application.Common.Exceptions;
using RealtimeDashboard.Application.Resources.Queries.GetAllResources;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Application.Resources.Queries.GetResourceById;

public class GetResourceByIdHandler : IRequestHandler<GetResourceByIdQuery, ResourceDto>
{
    public readonly IResourceRepository _repository;
    
    public GetResourceByIdHandler(IResourceRepository repository)
        => _repository = repository;

    public async Task<ResourceDto> Handle(GetResourceByIdQuery request, CancellationToken cancellationToken)
    {
        var resource = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Resource), request.Id);
        
        return new ResourceDto(
            resource.Id,
            resource.Name,
            resource.Description,
            resource.Category,
            resource.Location,
            resource.Status,
            resource.CreatedAt,
            resource.UpdatedAt
        );
    }
}