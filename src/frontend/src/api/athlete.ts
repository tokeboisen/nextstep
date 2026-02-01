import { apiClient } from './client';
import type {
  Athlete,
  CreateAthleteRequest,
  UpdatePersonalInfoRequest,
  UpdatePhysiologicalDataRequest,
  UpdateTrainingAccessRequest,
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
};
