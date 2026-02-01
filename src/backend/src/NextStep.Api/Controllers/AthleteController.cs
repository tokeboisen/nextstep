using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextStep.Application.AthleteProfile.Commands;
using NextStep.Application.AthleteProfile.DTOs;
using NextStep.Application.AthleteProfile.Queries;

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
        var command = new UpdatePhysiologicalDataCommand(request.MaxHeartRate, request.LactateThreshold);
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

    [HttpPut("heart-rate-zones")]
    public async Task<ActionResult> UpdateHeartRateZones([FromBody] UpdateHeartRateZonesRequest request, CancellationToken cancellationToken)
    {
        var zones = request.Zones.Select(z => new HeartRateZoneInput(z.ZoneNumber, z.Name, z.MinBpm, z.MaxBpm)).ToList();
        var command = new UpdateHeartRateZonesCommand(zones);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("pace-zones")]
    public async Task<ActionResult> UpdatePaceZones([FromBody] UpdatePaceZonesRequest request, CancellationToken cancellationToken)
    {
        var zones = request.Zones.Select(z => new PaceZoneInput(z.ZoneNumber, z.Name, z.MinPace, z.MaxPace)).ToList();
        var command = new UpdatePaceZonesCommand(zones);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}

public record CreateAthleteRequest(string Name, DateOnly BirthDate);
public record UpdatePersonalInfoRequest(string Name, DateOnly BirthDate);
public record UpdatePhysiologicalDataRequest(int? MaxHeartRate, int? LactateThreshold);
public record UpdateTrainingAccessRequest(bool HasTrackAccess);
public record UpdateHeartRateZonesRequest(List<HeartRateZoneRequest> Zones);
public record HeartRateZoneRequest(int ZoneNumber, string Name, int MinBpm, int MaxBpm);
public record UpdatePaceZonesRequest(List<PaceZoneRequest> Zones);
public record PaceZoneRequest(int ZoneNumber, string Name, string MinPace, string MaxPace);
