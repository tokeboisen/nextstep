import { apiClient } from './client';
import type {
  Athlete,
  CreateAthleteRequest,
  UpdatePersonalInfoRequest,
  UpdatePhysiologicalDataRequest,
  UpdateTrainingAccessRequest,
  UpdateTrainingAvailabilityRequest,
  AddGoalRequest,
  UpdateGoalRequest,
} from '../types/athlete';

export const athleteApi = {
  getProfile: async (): Promise<Athlete | null> => {
    try {
      const response = await apiClient.get<Athlete>('/athlete');
      return response.data;
    } catch (error: unknown) {
      if ((error as { response?: { status: number } }).response?.status === 404) {
        return null;
      }
      throw error;
    }
  },

  createProfile: async (request: CreateAthleteRequest): Promise<string> => {
    const response = await apiClient.post<string>('/athlete', request);
    return response.data;
  },

  updatePersonalInfo: async (request: UpdatePersonalInfoRequest): Promise<void> => {
    await apiClient.put('/athlete/personal-info', request);
  },

  updatePhysiologicalData: async (request: UpdatePhysiologicalDataRequest): Promise<void> => {
    await apiClient.put('/athlete/physiological-data', request);
  },

  updateTrainingAccess: async (request: UpdateTrainingAccessRequest): Promise<void> => {
    await apiClient.put('/athlete/training-access', request);
  },

  updateTrainingAvailability: async (request: UpdateTrainingAvailabilityRequest): Promise<void> => {
    await apiClient.put('/athlete/training-availability', request);
  },

  addGoal: async (request: AddGoalRequest): Promise<string> => {
    const response = await apiClient.post<string>('/athlete/goals', request);
    return response.data;
  },

  updateGoal: async (goalId: string, request: UpdateGoalRequest): Promise<void> => {
    await apiClient.put(`/athlete/goals/${goalId}`, request);
  },

  deleteGoal: async (goalId: string): Promise<void> => {
    await apiClient.delete(`/athlete/goals/${goalId}`);
  },

  setPrimaryGoal: async (goalId: string): Promise<void> => {
    await apiClient.put(`/athlete/goals/${goalId}/primary`);
  },
};
