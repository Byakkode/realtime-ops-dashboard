using MediatR;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Application.Resources.Queries.GetAllResources;

public class GetAllResourcesHandler : IRequestHandler<GetAllResourcesQuery, IReadOnlyList<ResourceDto>>
{
    private readonly IResourceRepository _repository;
    
    public GetAllResourcesHandler(IResourceRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyList<ResourceDto>> Handle(GetAllResourcesQuery request,
        CancellationToken cancellationToken)
    {
        var resources = request.Category is not null
            ? await _repository.GetByCategoryAsync(request.Category, cancellationToken)
            : await _repository.GetAllAsync(cancellationToken);

        return resources.Select(r => new ResourceDto(
            r.Id,
            r.Name,
            r.Description,
            r.Category,
            r.Location,
            r.Status,
            r.CreatedAt,
            r.UpdatedAt
        )).ToList();
    }
}