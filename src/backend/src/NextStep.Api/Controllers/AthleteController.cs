using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextStep.Application.AthleteProfile.Commands;
using NextStep.Application.AthleteProfile.DTOs;
using NextStep.Application.AthleteProfile.Queries;
using NextStep.Domain.AthleteProfile.ValueObjects;

namespace NextStep.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AthleteController : ControllerBase
{
    private readonly IMediator _mediator;

    public AthleteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<AthleteDto>> GetProfile(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAthleteProfileQuery(), cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProfile([FromBody] CreateAthleteRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateAthleteCommand(request.Name, request.BirthDate);
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProfile), new { id }, id);
    }

    [HttpPut("personal-info")]
    public async Task<ActionResult> UpdatePersonalInfo([FromBody] UpdatePersonalInfoRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdatePersonalInfoCommand(request.Name, request.BirthDate);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("physiological-data")]
    public async Task<ActionResult> UpdatePhysiologicalData([FromBody] UpdatePhysiologicalDataRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdatePhysiologicalDataCommand(request.MaxHeartRate, request.LactateThresholdHeartRate, request.LactateThresholdPace);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("training-access")]
    public async Task<ActionResult> UpdateTrainingAccess([FromBody] UpdateTrainingAccessRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTrainingAccessCommand(request.HasTrackAccess);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("training-availability")]
    public async Task<ActionResult> UpdateTrainingAvailability([FromBody] UpdateTrainingAvailabilityRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateTrainingAvailabilityCommand(
                request.Monday,
                request.Tuesday,
                request.Wednesday,
                request.Thursday,
                request.Friday,
                request.Saturday,
                request.Sunday);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public record CreateAthleteRequest(string Name, DateOnly BirthDate);
public record UpdatePersonalInfoRequest(string Name, DateOnly BirthDate);
public record UpdatePhysiologicalDataRequest(int? MaxHeartRate, int? LactateThresholdHeartRate, string? LactateThresholdPace);
public record UpdateTrainingAccessRequest(bool HasTrackAccess);
public record UpdateTrainingAvailabilityRequest(
    WorkoutType Monday,
    WorkoutType Tuesday,
    WorkoutType Wednesday,
    WorkoutType Thursday,
    WorkoutType Friday,
    WorkoutType Saturday,
    WorkoutType Sunday);
