using FluentAssertions;
using NSubstitute;
using NextStep.Application.AthleteProfile.Commands;
using NextStep.Application.Common;
using NextStep.Domain.AthleteProfile.Entities;

namespace NextStep.Application.Tests;

public class CreateAthleteCommandHandlerTests
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly CreateAthleteCommandHandler _handler;

    public CreateAthleteCommandHandlerTests()
    {
        _athleteRepository = Substitute.For<IAthleteRepository>();
        _handler = new CreateAthleteCommandHandler(_athleteRepository);
    }

    [Fact]
    public async Task Handle_WhenNoExistingAthlete_ShouldCreateAndReturnId()
    {
        // Arrange
        _athleteRepository.GetSingleAthleteAsync(Arg.Any<CancellationToken>())
            .Returns((Athlete?)null);

        var command = new CreateAthleteCommand("John Doe", new DateOnly(1990, 5, 15));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        await _athleteRepository.Received(1).AddAsync(Arg.Any<Athlete>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAthleteAlreadyExists_ShouldThrow()
    {
        // Arrange
        var existingAthlete = Athlete.Create("Existing", new DateOnly(1990, 1, 1));
        _athleteRepository.GetSingleAthleteAsync(Arg.Any<CancellationToken>())
            .Returns(existingAthlete);

        var command = new CreateAthleteCommand("John Doe", new DateOnly(1990, 5, 15));

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
    }
}
