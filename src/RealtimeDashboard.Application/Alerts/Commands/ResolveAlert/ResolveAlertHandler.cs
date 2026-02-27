using MediatR;
using RealtimeDashboard.Application.Common.Exceptions;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Application.Alerts.Commands.ResolveAlert;

public class ResolveAlertHandler : IRequestHandler<ResolveAlertCommand>
{
    private readonly IAlertRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ResolveAlertHandler(IAlertRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ResolveAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await _repository.GetByIdAsync(request.AlertId, cancellationToken)
                    ?? throw new NotFoundException(nameof(Alert), request.AlertId);

        alert.Resolve(request.ResolvedBy);
        _repository.Update(alert);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}