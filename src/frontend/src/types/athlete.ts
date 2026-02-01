export interface Athlete {
  id: string;
  personalInfo: PersonalInfo;
  physiologicalData: PhysiologicalData;
  trainingAccess: TrainingAccess;
  trainingAvailability: TrainingAvailability;
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
  lactateThresholdHeartRate: number | null;
  lactateThresholdPace: string | null;
}

export interface TrainingAccess {
  hasTrackAccess: boolean;
}

export interface TrainingAvailability {
  monday: WorkoutType;
  tuesday: WorkoutType;
  wednesday: WorkoutType;
  thursday: WorkoutType;
  friday: WorkoutType;
  saturday: WorkoutType;
  sunday: WorkoutType;
}

export type WorkoutType =
  | 'Rest'
  | 'CrossHIIT'
  | 'Recovery'
  | 'EasyRun'
  | 'Speed'
  | 'TempoRun'
  | 'LongRun';

export const WORKOUT_TYPES: { value: WorkoutType; label: string; isQuality: boolean }[] = [
  { value: 'Rest', label: 'Rest', isQuality: false },
  { value: 'CrossHIIT', label: 'Cross / HIIT', isQuality: true },
  { value: 'Recovery', label: 'Recovery', isQuality: false },
  { value: 'EasyRun', label: 'Easy Run', isQuality: false },
  { value: 'Speed', label: 'Speed', isQuality: true },
  { value: 'TempoRun', label: 'Tempo Run', isQuality: true },
  { value: 'LongRun', label: 'Long Run', isQuality: true },
];

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
  lactateThresholdHeartRate: number | null;
  lactateThresholdPace: string | null;
}

export interface UpdateTrainingAccessRequest {
  hasTrackAccess: boolean;
}

export interface UpdateTrainingAvailabilityRequest {
  monday: number;
  tuesday: number;
  wednesday: number;
  thursday: number;
  friday: number;
  saturday: number;
  sunday: number;
}
