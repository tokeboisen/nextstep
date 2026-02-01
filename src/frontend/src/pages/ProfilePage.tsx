import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import {
  useAthlete,
  useCreateAthlete,
  useUpdatePersonalInfo,
  useUpdatePhysiologicalData,
  useUpdateTrainingAccess,
  useUpdateTrainingAvailability,
} from '../hooks/useAthlete';
import {
  Box,
  Drawer,
  AppBar,
  Toolbar,
  Typography,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Card,
  CardContent,
  CardHeader,
  TextField,
  Button,
  IconButton,
  CircularProgress,
  Alert,
  Switch,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Divider,
  FormControl,
  Select,
  MenuItem,
  Chip,
} from '@mui/material';
import PersonIcon from '@mui/icons-material/Person';
import FavoriteIcon from '@mui/icons-material/Favorite';
import DirectionsRunIcon from '@mui/icons-material/DirectionsRun';
import SpeedIcon from '@mui/icons-material/Speed';
import TimerIcon from '@mui/icons-material/Timer';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import LogoutIcon from '@mui/icons-material/Logout';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import CloseIcon from '@mui/icons-material/Close';
import { WORKOUT_TYPES, type WorkoutType } from '../types/athlete';

const DRAWER_WIDTH = 280;

type SectionId = 'personal' | 'physiological' | 'training' | 'availability' | 'heartRateZones' | 'paceZones';

const DAYS_OF_WEEK = [
  { key: 'monday' as const, label: 'Monday' },
  { key: 'tuesday' as const, label: 'Tuesday' },
  { key: 'wednesday' as const, label: 'Wednesday' },
  { key: 'thursday' as const, label: 'Thursday' },
  { key: 'friday' as const, label: 'Friday' },
  { key: 'saturday' as const, label: 'Saturday' },
  { key: 'sunday' as const, label: 'Sunday' },
];

const WORKOUT_TYPE_TO_NUMBER: Record<WorkoutType, number> = {
  'Rest': 0,
  'CrossHIIT': 1,
  'Recovery': 2,
  'EasyRun': 3,
  'Speed': 4,
  'TempoRun': 5,
  'LongRun': 6,
};

export function ProfilePage() {
  const { logout } = useAuth();
  const { data: athlete, isLoading, error } = useAthlete();
  const [activeSection, setActiveSection] = useState<SectionId>('personal');

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '100vh' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '100vh' }}>
        <Alert severity="error">Error loading profile</Alert>
      </Box>
    );
  }

  if (!athlete) {
    return <CreateAthleteForm />;
  }

  const navItems = [
    { id: 'personal' as const, label: 'Personal Info', icon: <PersonIcon /> },
    { id: 'physiological' as const, label: 'Physiological Data', icon: <FavoriteIcon /> },
    { id: 'training' as const, label: 'Training Access', icon: <DirectionsRunIcon /> },
    { id: 'availability' as const, label: 'Weekly Schedule', icon: <CalendarMonthIcon /> },
    { id: 'heartRateZones' as const, label: 'Heart Rate Zones', icon: <SpeedIcon /> },
    { id: 'paceZones' as const, label: 'Pace Zones', icon: <TimerIcon /> },
  ];

  return (
    <Box sx={{ display: 'flex' }}>
      {/* Sidebar */}
      <Drawer
        variant="permanent"
        sx={{
          width: DRAWER_WIDTH,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: DRAWER_WIDTH,
            boxSizing: 'border-box',
            bgcolor: 'primary.main',
            color: 'white',
          },
        }}
      >
        <Toolbar sx={{ px: 2, py: 3 }}>
          <DirectionsRunIcon sx={{ mr: 1.5, fontSize: 32 }} />
          <Typography variant="h5" sx={{ fontWeight: 600 }}>
            NextStep
          </Typography>
        </Toolbar>
        <Divider sx={{ borderColor: 'rgba(255,255,255,0.2)' }} />
        <List sx={{ flex: 1, py: 2 }}>
          {navItems.map((item) => (
            <ListItem key={item.id} disablePadding sx={{ mb: 0.5 }}>
              <ListItemButton
                selected={activeSection === item.id}
                onClick={() => setActiveSection(item.id)}
                sx={{
                  mx: 1,
                  borderRadius: 2,
                  '&.Mui-selected': {
                    bgcolor: 'rgba(255,255,255,0.2)',
                    '&:hover': {
                      bgcolor: 'rgba(255,255,255,0.25)',
                    },
                  },
                  '&:hover': {
                    bgcolor: 'rgba(255,255,255,0.1)',
                  },
                }}
              >
                <ListItemIcon sx={{ color: 'inherit', minWidth: 40 }}>
                  {item.icon}
                </ListItemIcon>
                <ListItemText primary={item.label} />
              </ListItemButton>
            </ListItem>
          ))}
        </List>
        <Divider sx={{ borderColor: 'rgba(255,255,255,0.2)' }} />
        <List sx={{ py: 1 }}>
          <ListItem disablePadding>
            <ListItemButton onClick={logout} sx={{ mx: 1, borderRadius: 2 }}>
              <ListItemIcon sx={{ color: 'inherit', minWidth: 40 }}>
                <LogoutIcon />
              </ListItemIcon>
              <ListItemText primary="Logout" />
            </ListItemButton>
          </ListItem>
        </List>
      </Drawer>

      {/* Main content */}
      <Box component="main" sx={{ flexGrow: 1, bgcolor: 'background.default', minHeight: '100vh' }}>
        <AppBar position="static" elevation={0} sx={{ bgcolor: 'white', borderBottom: 1, borderColor: 'divider' }}>
          <Toolbar>
            <Typography variant="h6" color="text.primary" sx={{ fontWeight: 500 }}>
              {navItems.find(n => n.id === activeSection)?.label}
            </Typography>
          </Toolbar>
        </AppBar>
        <Box sx={{ p: 3 }}>
          {activeSection === 'personal' && <PersonalInfoSection athlete={athlete} />}
          {activeSection === 'physiological' && <PhysiologicalDataSection athlete={athlete} />}
          {activeSection === 'training' && <TrainingAccessSection athlete={athlete} />}
          {activeSection === 'availability' && <TrainingAvailabilitySection athlete={athlete} />}
          {activeSection === 'heartRateZones' && <HeartRateZonesSection athlete={athlete} />}
          {activeSection === 'paceZones' && <PaceZonesSection athlete={athlete} />}
        </Box>
      </Box>
    </Box>
  );
}

function CreateAthleteForm() {
  const [name, setName] = useState('');
  const [birthDate, setBirthDate] = useState('');
  const createAthlete = useCreateAthlete();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    createAthlete.mutate({ name, birthDate });
  };

  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '100vh',
        bgcolor: 'background.default',
        p: 3,
      }}
    >
      <Card sx={{ maxWidth: 500, width: '100%' }}>
        <CardHeader
          title="Create Your Profile"
          subheader="Let's set up your athlete profile to get started"
        />
        <CardContent>
          <form onSubmit={handleSubmit}>
            <TextField
              label="Name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              sx={{ mb: 2 }}
            />
            <TextField
              label="Birth Date"
              type="date"
              value={birthDate}
              onChange={(e) => setBirthDate(e.target.value)}
              required
              slotProps={{ inputLabel: { shrink: true } }}
              sx={{ mb: 3 }}
            />
            <Button
              type="submit"
              variant="contained"
              fullWidth
              size="large"
              disabled={createAthlete.isPending}
            >
              {createAthlete.isPending ? <CircularProgress size={24} /> : 'Create Profile'}
            </Button>
          </form>
        </CardContent>
      </Card>
    </Box>
  );
}

function PersonalInfoSection({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  const [isEditing, setIsEditing] = useState(false);
  const [name, setName] = useState(athlete.personalInfo.name);
  const [birthDate, setBirthDate] = useState(athlete.personalInfo.birthDate);
  const updatePersonalInfo = useUpdatePersonalInfo();

  const handleSave = () => {
    updatePersonalInfo.mutate({ name, birthDate }, {
      onSuccess: () => setIsEditing(false),
    });
  };

  const handleCancel = () => {
    setName(athlete.personalInfo.name);
    setBirthDate(athlete.personalInfo.birthDate);
    setIsEditing(false);
  };

  return (
    <Card sx={{ maxWidth: 600 }}>
      <CardHeader
        title="Personal Information"
        action={
          !isEditing && (
            <IconButton onClick={() => setIsEditing(true)}>
              <EditIcon />
            </IconButton>
          )
        }
      />
      <CardContent>
        {isEditing ? (
          <Box>
            <TextField
              label="Name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              sx={{ mb: 2 }}
            />
            <TextField
              label="Birth Date"
              type="date"
              value={birthDate}
              onChange={(e) => setBirthDate(e.target.value)}
              slotProps={{ inputLabel: { shrink: true } }}
              sx={{ mb: 3 }}
            />
            <Box sx={{ display: 'flex', gap: 1 }}>
              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                onClick={handleSave}
                disabled={updatePersonalInfo.isPending}
              >
                Save
              </Button>
              <Button
                variant="outlined"
                startIcon={<CloseIcon />}
                onClick={handleCancel}
              >
                Cancel
              </Button>
            </Box>
          </Box>
        ) : (
          <Box>
            <DataRow label="Name" value={athlete.personalInfo.name} />
            <DataRow label="Birth Date" value={athlete.personalInfo.birthDate} />
            <DataRow label="Age" value={`${athlete.personalInfo.age} years`} />
          </Box>
        )}
      </CardContent>
    </Card>
  );
}

function PhysiologicalDataSection({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  const [isEditing, setIsEditing] = useState(false);
  const [maxHeartRate, setMaxHeartRate] = useState<string>(athlete.physiologicalData.maxHeartRate?.toString() || '');
  const [lactateThresholdHeartRate, setLactateThresholdHeartRate] = useState<string>(athlete.physiologicalData.lactateThresholdHeartRate?.toString() || '');
  const [lactateThresholdPace, setLactateThresholdPace] = useState<string>(athlete.physiologicalData.lactateThresholdPace || '');
  const updatePhysiologicalData = useUpdatePhysiologicalData();

  const handleSave = () => {
    updatePhysiologicalData.mutate({
      maxHeartRate: maxHeartRate ? parseInt(maxHeartRate) : null,
      lactateThresholdHeartRate: lactateThresholdHeartRate ? parseInt(lactateThresholdHeartRate) : null,
      lactateThresholdPace: lactateThresholdPace || null,
    }, {
      onSuccess: () => setIsEditing(false),
    });
  };

  const handleCancel = () => {
    setMaxHeartRate(athlete.physiologicalData.maxHeartRate?.toString() || '');
    setLactateThresholdHeartRate(athlete.physiologicalData.lactateThresholdHeartRate?.toString() || '');
    setLactateThresholdPace(athlete.physiologicalData.lactateThresholdPace || '');
    setIsEditing(false);
  };

  return (
    <Card sx={{ maxWidth: 600 }}>
      <CardHeader
        title="Physiological Data"
        subheader="Your heart rate and pace thresholds for zone calculation"
        action={
          !isEditing && (
            <IconButton onClick={() => setIsEditing(true)}>
              <EditIcon />
            </IconButton>
          )
        }
      />
      <CardContent>
        {isEditing ? (
          <Box>
            <TextField
              label="Max Heart Rate"
              type="number"
              value={maxHeartRate}
              onChange={(e) => setMaxHeartRate(e.target.value)}
              slotProps={{ input: { endAdornment: <Typography color="text.secondary">bpm</Typography> } }}
              sx={{ mb: 2 }}
            />
            <TextField
              label="Lactate Threshold Heart Rate"
              type="number"
              value={lactateThresholdHeartRate}
              onChange={(e) => setLactateThresholdHeartRate(e.target.value)}
              slotProps={{ input: { endAdornment: <Typography color="text.secondary">bpm</Typography> } }}
              sx={{ mb: 2 }}
            />
            <TextField
              label="Lactate Threshold Pace"
              value={lactateThresholdPace}
              onChange={(e) => setLactateThresholdPace(e.target.value)}
              placeholder="e.g. 4:30"
              slotProps={{ input: { endAdornment: <Typography color="text.secondary">min/km</Typography> } }}
              sx={{ mb: 3 }}
            />
            <Box sx={{ display: 'flex', gap: 1 }}>
              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                onClick={handleSave}
                disabled={updatePhysiologicalData.isPending}
              >
                Save
              </Button>
              <Button
                variant="outlined"
                startIcon={<CloseIcon />}
                onClick={handleCancel}
              >
                Cancel
              </Button>
            </Box>
          </Box>
        ) : (
          <Box>
            <DataRow
              label="Max Heart Rate"
              value={athlete.physiologicalData.maxHeartRate ? `${athlete.physiologicalData.maxHeartRate} bpm` : 'Not set'}
            />
            <DataRow
              label="Lactate Threshold Heart Rate"
              value={athlete.physiologicalData.lactateThresholdHeartRate ? `${athlete.physiologicalData.lactateThresholdHeartRate} bpm` : 'Not set'}
            />
            <DataRow
              label="Lactate Threshold Pace"
              value={athlete.physiologicalData.lactateThresholdPace ? `${athlete.physiologicalData.lactateThresholdPace} min/km` : 'Not set'}
            />
          </Box>
        )}
      </CardContent>
    </Card>
  );
}

function TrainingAccessSection({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  const updateTrainingAccess = useUpdateTrainingAccess();

  const handleToggle = () => {
    updateTrainingAccess.mutate({ hasTrackAccess: !athlete.trainingAccess.hasTrackAccess });
  };

  return (
    <Card sx={{ maxWidth: 600 }}>
      <CardHeader
        title="Training Access"
        subheader="Configure your available training facilities"
      />
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Box>
            <Typography variant="body1" fontWeight={500}>Track Access</Typography>
            <Typography variant="body2" color="text.secondary">
              Do you have access to a running track?
            </Typography>
          </Box>
          <Switch
            checked={athlete.trainingAccess.hasTrackAccess}
            onChange={handleToggle}
            disabled={updateTrainingAccess.isPending}
          />
        </Box>
      </CardContent>
    </Card>
  );
}

function TrainingAvailabilitySection({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  const [isEditing, setIsEditing] = useState(false);
  const [schedule, setSchedule] = useState({
    monday: athlete.trainingAvailability.monday,
    tuesday: athlete.trainingAvailability.tuesday,
    wednesday: athlete.trainingAvailability.wednesday,
    thursday: athlete.trainingAvailability.thursday,
    friday: athlete.trainingAvailability.friday,
    saturday: athlete.trainingAvailability.saturday,
    sunday: athlete.trainingAvailability.sunday,
  });
  const [error, setError] = useState<string | null>(null);
  const updateTrainingAvailability = useUpdateTrainingAvailability();

  const handleChange = (day: keyof typeof schedule, value: WorkoutType) => {
    setSchedule(prev => ({ ...prev, [day]: value }));
    setError(null);
  };

  const handleSave = () => {
    setError(null);
    updateTrainingAvailability.mutate({
      monday: WORKOUT_TYPE_TO_NUMBER[schedule.monday],
      tuesday: WORKOUT_TYPE_TO_NUMBER[schedule.tuesday],
      wednesday: WORKOUT_TYPE_TO_NUMBER[schedule.wednesday],
      thursday: WORKOUT_TYPE_TO_NUMBER[schedule.thursday],
      friday: WORKOUT_TYPE_TO_NUMBER[schedule.friday],
      saturday: WORKOUT_TYPE_TO_NUMBER[schedule.saturday],
      sunday: WORKOUT_TYPE_TO_NUMBER[schedule.sunday],
    }, {
      onSuccess: () => setIsEditing(false),
      onError: (err: unknown) => {
        const errorMessage = (err as { response?: { data?: { error?: string } } })?.response?.data?.error
          || 'Failed to save schedule';
        setError(errorMessage);
      },
    });
  };

  const handleCancel = () => {
    setSchedule({
      monday: athlete.trainingAvailability.monday,
      tuesday: athlete.trainingAvailability.tuesday,
      wednesday: athlete.trainingAvailability.wednesday,
      thursday: athlete.trainingAvailability.thursday,
      friday: athlete.trainingAvailability.friday,
      saturday: athlete.trainingAvailability.saturday,
      sunday: athlete.trainingAvailability.sunday,
    });
    setError(null);
    setIsEditing(false);
  };

  const getWorkoutTypeInfo = (type: WorkoutType) => {
    return WORKOUT_TYPES.find(wt => wt.value === type) || WORKOUT_TYPES[0];
  };

  return (
    <Card sx={{ maxWidth: 700 }}>
      <CardHeader
        title="Weekly Training Schedule"
        subheader="Plan your training for each day of the week. Quality workouts cannot be scheduled on consecutive days."
        action={
          !isEditing && (
            <IconButton onClick={() => setIsEditing(true)}>
              <EditIcon />
            </IconButton>
          )
        }
      />
      <CardContent>
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        {isEditing ? (
          <Box>
            <TableContainer component={Paper} variant="outlined" sx={{ mb: 2 }}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Day</TableCell>
                    <TableCell>Workout Type</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {DAYS_OF_WEEK.map((day) => (
                    <TableRow key={day.key}>
                      <TableCell sx={{ fontWeight: 500 }}>{day.label}</TableCell>
                      <TableCell>
                        <FormControl fullWidth size="small">
                          <Select
                            value={schedule[day.key]}
                            onChange={(e) => handleChange(day.key, e.target.value as WorkoutType)}
                          >
                            {WORKOUT_TYPES.map((wt) => (
                              <MenuItem key={wt.value} value={wt.value}>
                                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                  {wt.label}
                                  {wt.isQuality && (
                                    <Chip label="Quality" size="small" color="primary" sx={{ height: 20 }} />
                                  )}
                                </Box>
                              </MenuItem>
                            ))}
                          </Select>
                        </FormControl>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
            <Box sx={{ display: 'flex', gap: 1 }}>
              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                onClick={handleSave}
                disabled={updateTrainingAvailability.isPending}
              >
                Save
              </Button>
              <Button
                variant="outlined"
                startIcon={<CloseIcon />}
                onClick={handleCancel}
              >
                Cancel
              </Button>
            </Box>
          </Box>
        ) : (
          <TableContainer component={Paper} variant="outlined">
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Day</TableCell>
                  <TableCell>Workout Type</TableCell>
                  <TableCell>Category</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {DAYS_OF_WEEK.map((day) => {
                  const workoutInfo = getWorkoutTypeInfo(athlete.trainingAvailability[day.key]);
                  return (
                    <TableRow key={day.key}>
                      <TableCell sx={{ fontWeight: 500 }}>{day.label}</TableCell>
                      <TableCell>{workoutInfo.label}</TableCell>
                      <TableCell>
                        {workoutInfo.isQuality ? (
                          <Chip label="Quality" size="small" color="primary" />
                        ) : (
                          <Chip label="Easy" size="small" color="default" variant="outlined" />
                        )}
                      </TableCell>
                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </CardContent>
    </Card>
  );
}

function HeartRateZonesSection({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  return (
    <Card sx={{ maxWidth: 700 }}>
      <CardHeader
        title="Heart Rate Zones"
        subheader="Calculated automatically from Lactate Threshold Heart Rate"
      />
      <CardContent>
        {athlete.heartRateZones.length > 0 ? (
          <TableContainer component={Paper} variant="outlined">
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Zone</TableCell>
                  <TableCell>Name</TableCell>
                  <TableCell align="right">Min BPM</TableCell>
                  <TableCell align="right">Max BPM</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {athlete.heartRateZones.map((zone) => (
                  <TableRow key={zone.zoneNumber}>
                    <TableCell>{zone.zoneNumber}</TableCell>
                    <TableCell>{zone.name}</TableCell>
                    <TableCell align="right">{zone.minBpm}</TableCell>
                    <TableCell align="right">{zone.maxBpm}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        ) : (
          <Alert severity="info">
            Set your Lactate Threshold Heart Rate in Physiological Data to calculate zones
          </Alert>
        )}
      </CardContent>
    </Card>
  );
}

function PaceZonesSection({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  return (
    <Card sx={{ maxWidth: 700 }}>
      <CardHeader
        title="Pace Zones"
        subheader="Calculated automatically from Lactate Threshold Pace"
      />
      <CardContent>
        {athlete.paceZones.length > 0 ? (
          <TableContainer component={Paper} variant="outlined">
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Zone</TableCell>
                  <TableCell>Name</TableCell>
                  <TableCell align="right">Min Pace</TableCell>
                  <TableCell align="right">Max Pace</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {athlete.paceZones.map((zone) => (
                  <TableRow key={zone.zoneNumber}>
                    <TableCell>{zone.zoneNumber}</TableCell>
                    <TableCell>{zone.name}</TableCell>
                    <TableCell align="right">{zone.minPace}</TableCell>
                    <TableCell align="right">{zone.maxPace}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        ) : (
          <Alert severity="info">
            Set your Lactate Threshold Pace in Physiological Data to calculate zones
          </Alert>
        )}
      </CardContent>
    </Card>
  );
}

function DataRow({ label, value }: { label: string; value: string }) {
  return (
    <Box sx={{ display: 'flex', py: 1.5, borderBottom: 1, borderColor: 'divider', '&:last-child': { borderBottom: 0 } }}>
      <Typography variant="body2" color="text.secondary" sx={{ minWidth: 200 }}>
        {label}
      </Typography>
      <Typography variant="body1">{value}</Typography>
    </Box>
  );
}
