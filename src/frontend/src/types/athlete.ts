export interface Athlete {
  id: string;
  personalInfo: PersonalInfo;
  physiologicalData: PhysiologicalData;
  trainingAccess: TrainingAccess;
  trainingAvailability: TrainingAvailability;
  heartRateZones: HeartRateZone[];
  paceZones: PaceZone[];
  goals: Goal[];
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

export interface Goal {
  id: string;
  raceDate: string;
  targetTime: string;
  distanceType: DistanceType;
  customDistanceKm: number | null;
  distanceDisplay: string;
  isPrimary: boolean;
}

export type DistanceType =
  | 'Distance1600m'
  | 'Distance5K'
  | 'Distance10K'
  | 'Distance16K'
  | 'HalfMarathon'
  | 'Marathon'
  | 'Custom';

export const DISTANCE_TYPES: { value: DistanceType; label: string; km: number | null }[] = [
  { value: 'Distance1600m', label: '1600m', km: 1.6 },
  { value: 'Distance5K', label: '5K', km: 5 },
  { value: 'Distance10K', label: '10K', km: 10 },
  { value: 'Distance16K', label: '16K', km: 16 },
  { value: 'HalfMarathon', label: 'Half Marathon', km: 21.0975 },
  { value: 'Marathon', label: 'Marathon', km: 42.195 },
  { value: 'Custom', label: 'Custom', km: null },
];

export interface AddGoalRequest {
  raceDate: string;
  targetTime: string;
  distanceType: number;
  customDistanceKm: number | null;
}

export interface UpdateGoalRequest {
  raceDate: string;
  targetTime: string;
  distanceType: number;
  customDistanceKm: number | null;
}
