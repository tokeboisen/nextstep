import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { athleteApi } from '../api/athlete';
import type {
  CreateAthleteRequest,
  UpdatePersonalInfoRequest,
  UpdatePhysiologicalDataRequest,
  UpdateTrainingAccessRequest,
  UpdateHeartRateZonesRequest,
  UpdatePaceZonesRequest,
} from '../types/athlete';

export function useAthlete() {
  return useQuery({
    queryKey: ['athlete'],
    queryFn: athleteApi.getProfile,
  });
}

export function useCreateAthlete() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: CreateAthleteRequest) => athleteApi.createProfile(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}

export function useUpdatePersonalInfo() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: UpdatePersonalInfoRequest) => athleteApi.updatePersonalInfo(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}

export function useUpdatePhysiologicalData() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: UpdatePhysiologicalDataRequest) => athleteApi.updatePhysiologicalData(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}

export function useUpdateTrainingAccess() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: UpdateTrainingAccessRequest) => athleteApi.updateTrainingAccess(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}

export function useUpdateHeartRateZones() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: UpdateHeartRateZonesRequest) => athleteApi.updateHeartRateZones(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}

export function useUpdatePaceZones() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: UpdatePaceZonesRequest) => athleteApi.updatePaceZones(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}
