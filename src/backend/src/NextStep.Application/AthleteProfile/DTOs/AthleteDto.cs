namespace NextStep.Application.AthleteProfile.DTOs;

public record AthleteDto(
    Guid Id,
    PersonalInfoDto PersonalInfo,
    PhysiologicalDataDto PhysiologicalData,
    TrainingAccessDto TrainingAccess,
    List<HeartRateZoneDto> HeartRateZones,
    List<PaceZoneDto> PaceZones
);

public record PersonalInfoDto(
    string Name,
    DateOnly BirthDate,
    int Age
);

public record PhysiologicalDataDto(
    int? MaxHeartRate,
    int? LactateThresholdHeartRate,
    string? LactateThresholdPace
);

public record TrainingAccessDto(
    bool HasTrackAccess
);

public record HeartRateZoneDto(
    int ZoneNumber,
    string Name,
    int MinBpm,
    int MaxBpm
);

public record PaceZoneDto(
    int ZoneNumber,
    string Name,
    string MinPace,
    string MaxPace
);
