export interface Athlete {
  id: string;
  personalInfo: PersonalInfo;
  physiologicalData: PhysiologicalData;
  trainingAccess: TrainingAccess;
  heartRateZones: HeartRateZone[];
  paceZones: PaceZone[];
}

export interface PersonalInfo {
  name: string;
  birthDate: string;
  age: number;
}

export interface PhysiologicalData {
  maxHeartRate: number | null;
  lactateThreshold: number | null;
}

export interface TrainingAccess {
  hasTrackAccess: boolean;
}

export interface HeartRateZone {
  zoneNumber: number;
  name: string;
  minBpm: number;
  maxBpm: number;
}

export interface PaceZone {
  zoneNumber: number;
  name: string;
  minPace: string;
  maxPace: string;
}

export interface CreateAthleteRequest {
  name: string;
  birthDate: string;
}

export interface UpdatePersonalInfoRequest {
  name: string;
  birthDate: string;
}

export interface UpdatePhysiologicalDataRequest {
  maxHeartRate: number | null;
  lactateThreshold: number | null;
}

export interface UpdateTrainingAccessRequest {
  hasTrackAccess: boolean;
}

export interface UpdateHeartRateZonesRequest {
  zones: HeartRateZone[];
}

export interface UpdatePaceZonesRequest {
  zones: PaceZone[];
}
