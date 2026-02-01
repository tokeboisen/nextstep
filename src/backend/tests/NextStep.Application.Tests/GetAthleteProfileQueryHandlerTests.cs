using FluentAssertions;
using NSubstitute;
using NextStep.Application.AthleteProfile.Queries;
using NextStep.Application.Common;
using NextStep.Domain.AthleteProfile.Entities;

namespace NextStep.Application.Tests;

public class GetAthleteProfileQueryHandlerTests
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly GetAthleteProfileQueryHandler _handler;

    public GetAthleteProfileQueryHandlerTests()
    {
        _athleteRepository = Substitute.For<IAthleteRepository>();
        _handler = new GetAthleteProfileQueryHandler(_athleteRepository);
    }

    [Fact]
    public async Task Handle_WhenAthleteExists_ShouldReturnDto()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        athlete.UpdatePhysiologicalData(185, 165);
        athlete.UpdateTrainingAccess(true);

        _athleteRepository.GetSingleAthleteAsync(Arg.Any<CancellationToken>())
            .Returns(athlete);

        // Act
        var result = await _handler.Handle(new GetAthleteProfileQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(athlete.Id);
        result.PersonalInfo.Name.Should().Be("John Doe");
        result.PhysiologicalData.MaxHeartRate.Should().Be(185);
        result.TrainingAccess.HasTrackAccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenAthleteDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _athleteRepository.GetSingleAthleteAsync(Arg.Any<CancellationToken>())
            .Returns((Athlete?)null);

        // Act
        var result = await _handler.Handle(new GetAthleteProfileQuery(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
