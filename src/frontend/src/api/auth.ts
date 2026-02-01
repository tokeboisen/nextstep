import { apiClient } from './client';
import type { LoginRequest, LoginResponse } from '../types/auth';

export const authApi = {
  login: async (request: LoginRequest): Promise<LoginResponse> => {
    const response = await apiClient.post<LoginResponse>('/auth/login', request);
    return response.data;
  },
};
