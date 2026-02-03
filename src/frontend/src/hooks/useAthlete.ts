import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { athleteApi } from '../api/athlete';
import type {
  CreateAthleteRequest,
  UpdatePersonalInfoRequest,
  UpdatePhysiologicalDataRequest,
  UpdateTrainingAccessRequest,
  UpdateTrainingAvailabilityRequest,
  AddGoalRequest,
  UpdateGoalRequest,
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

export function useUpdateTrainingAvailability() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: UpdateTrainingAvailabilityRequest) => athleteApi.updateTrainingAvailability(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}

export function useAddGoal() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: AddGoalRequest) => athleteApi.addGoal(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}

export function useUpdateGoal() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ goalId, request }: { goalId: string; request: UpdateGoalRequest }) =>
      athleteApi.updateGoal(goalId, request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}

export function useDeleteGoal() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (goalId: string) => athleteApi.deleteGoal(goalId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}

export function useSetPrimaryGoal() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (goalId: string) => athleteApi.setPrimaryGoal(goalId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['athlete'] });
    },
  });
}
