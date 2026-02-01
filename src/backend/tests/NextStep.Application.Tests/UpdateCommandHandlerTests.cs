using FluentAssertions;
using NSubstitute;
using NextStep.Application.AthleteProfile.Commands;
using NextStep.Application.Common;
using NextStep.Domain.AthleteProfile.Entities;

namespace NextStep.Application.Tests;

public class UpdatePersonalInfoCommandHandlerTests
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly UpdatePersonalInfoCommandHandler _handler;

    public UpdatePersonalInfoCommandHandlerTests()
    {
        _athleteRepository = Substitute.For<IAthleteRepository>();
        _handler = new UpdatePersonalInfoCommandHandler(_athleteRepository);
    }

    [Fact]
    public async Task Handle_WhenAthleteExists_ShouldUpdateAndSave()
    {
        // Arrange
        var athlete = Athlete.Create("Old Name", new DateOnly(1990, 1, 1));
        _athleteRepository.GetSingleAthleteAsync(Arg.Any<CancellationToken>())
            .Returns(athlete);

        var command = new UpdatePersonalInfoCommand("New Name", new DateOnly(1985, 5, 15));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        athlete.PersonalInfo.Name.Should().Be("New Name");
        athlete.PersonalInfo.BirthDate.Should().Be(new DateOnly(1985, 5, 15));
        await _athleteRepository.Received(1).UpdateAsync(athlete, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAthleteDoesNotExist_ShouldThrow()
    {
        // Arrange
        _athleteRepository.GetSingleAthleteAsync(Arg.Any<CancellationToken>())
            .Returns((Athlete?)null);

        var command = new UpdatePersonalInfoCommand("Name", new DateOnly(1990, 1, 1));

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*not found*");
    }
}

public class UpdatePhysiologicalDataCommandHandlerTests
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly UpdatePhysiologicalDataCommandHandler _handler;

    public UpdatePhysiologicalDataCommandHandlerTests()
    {
        _athleteRepository = Substitute.For<IAthleteRepository>();
        _handler = new UpdatePhysiologicalDataCommandHandler(_athleteRepository);
    }

    [Fact]
    public async Task Handle_WhenAthleteExists_ShouldUpdateAndSave()
    {
        // Arrange
        var athlete = Athlete.Create("Name", new DateOnly(1990, 1, 1));
        _athleteRepository.GetSingleAthleteAsync(Arg.Any<CancellationToken>())
            .Returns(athlete);

        var command = new UpdatePhysiologicalDataCommand(185, 165);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        athlete.PhysiologicalData.MaxHeartRate.Should().Be(185);
        athlete.PhysiologicalData.LactateThreshold.Should().Be(165);
        await _athleteRepository.Received(1).UpdateAsync(athlete, Arg.Any<CancellationToken>());
    }
}

public class UpdateTrainingAccessCommandHandlerTests
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly UpdateTrainingAccessCommandHandler _handler;

    public UpdateTrainingAccessCommandHandlerTests()
    {
        _athleteRepository = Substitute.For<IAthleteRepository>();
        _handler = new UpdateTrainingAccessCommandHandler(_athleteRepository);
    }

    [Fact]
    public async Task Handle_WhenAthleteExists_ShouldUpdateAndSave()
    {
        // Arrange
        var athlete = Athlete.Create("Name", new DateOnly(1990, 1, 1));
        _athleteRepository.GetSingleAthleteAsync(Arg.Any<CancellationToken>())
            .Returns(athlete);

        var command = new UpdateTrainingAccessCommand(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        athlete.TrainingAccess.HasTrackAccess.Should().BeTrue();
        await _athleteRepository.Received(1).UpdateAsync(athlete, Arg.Any<CancellationToken>());
    }
}
