using NSubstitute;
using RealtimeDashboard.Application.Resources.Commands.CreateResource;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.UnitTests.Application;

public class CreateResourceHandlerTests
{
    private readonly IResourceRepository _repository = Substitute.For<IResourceRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task Handle_WithValidCommand_ShouldAddResourceAndReturnId()
    {
        var handler = new CreateResourceHandler(_repository, _unitOfWork);
        var command = new CreateResourceCommand("Bed 101", "ICU Bed", "Bed", "Ward A");

        var id = await handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        await _repository.Received(1).AddAsync(
            Arg.Is<Resource>(r => r.Name == "Bed 101"),
            Arg.Any<CancellationToken>()
        );
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}