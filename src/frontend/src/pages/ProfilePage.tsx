import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import {
  useAthlete,
  useCreateAthlete,
  useUpdatePersonalInfo,
  useUpdatePhysiologicalData,
  useUpdateTrainingAccess,
} from '../hooks/useAthlete';

export function ProfilePage() {
  const { logout } = useAuth();
  const { data: athlete, isLoading, error } = useAthlete();

  if (isLoading) {
    return <div className="loading">Loading...</div>;
  }

  if (error) {
    return <div className="error">Error loading profile</div>;
  }

  return (
    <div className="profile-container">
      <header className="profile-header">
        <h1>NextStep</h1>
        <button onClick={logout} className="logout-btn">
          Logout
        </button>
      </header>

      <main className="profile-content">
        {athlete ? (
          <AthleteProfile athlete={athlete} />
        ) : (
          <CreateAthleteForm />
        )}
      </main>
    </div>
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
    <div className="card">
      <h2>Create Your Profile</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="name">Name</label>
          <input
            type="text"
            id="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="birthDate">Birth Date</label>
          <input
            type="date"
            id="birthDate"
            value={birthDate}
            onChange={(e) => setBirthDate(e.target.value)}
            required
          />
        </div>
        <button type="submit" disabled={createAthlete.isPending}>
          {createAthlete.isPending ? 'Creating...' : 'Create Profile'}
        </button>
      </form>
    </div>
  );
}

function AthleteProfile({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  return (
    <div className="profile-sections">
      <PersonalInfoSection athlete={athlete} />
      <PhysiologicalDataSection athlete={athlete} />
      <TrainingAccessSection athlete={athlete} />
      <HeartRateZonesSection athlete={athlete} />
      <PaceZonesSection athlete={athlete} />
    </div>
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

  return (
    <div className="card">
      <h2>Personal Info</h2>
      {isEditing ? (
        <>
          <div className="form-group">
            <label>Name</label>
            <input value={name} onChange={(e) => setName(e.target.value)} />
          </div>
          <div className="form-group">
            <label>Birth Date</label>
            <input type="date" value={birthDate} onChange={(e) => setBirthDate(e.target.value)} />
          </div>
          <div className="button-group">
            <button onClick={handleSave} disabled={updatePersonalInfo.isPending}>Save</button>
            <button onClick={() => setIsEditing(false)} className="secondary">Cancel</button>
          </div>
        </>
      ) : (
        <>
          <dl>
            <dt>Name</dt>
            <dd>{athlete.personalInfo.name}</dd>
            <dt>Birth Date</dt>
            <dd>{athlete.personalInfo.birthDate}</dd>
            <dt>Age</dt>
            <dd>{athlete.personalInfo.age} years</dd>
          </dl>
          <button onClick={() => setIsEditing(true)}>Edit</button>
        </>
      )}
    </div>
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

  return (
    <div className="card">
      <h2>Physiological Data</h2>
      {isEditing ? (
        <>
          <div className="form-group">
            <label>Max Heart Rate (bpm)</label>
            <input type="number" value={maxHeartRate} onChange={(e) => setMaxHeartRate(e.target.value)} />
          </div>
          <div className="form-group">
            <label>Lactate Threshold Heart Rate (bpm)</label>
            <input type="number" value={lactateThresholdHeartRate} onChange={(e) => setLactateThresholdHeartRate(e.target.value)} />
          </div>
          <div className="form-group">
            <label>Lactate Threshold Pace (mm:ss)</label>
            <input
              type="text"
              value={lactateThresholdPace}
              onChange={(e) => setLactateThresholdPace(e.target.value)}
              placeholder="e.g. 4:30"
            />
          </div>
          <div className="button-group">
            <button onClick={handleSave} disabled={updatePhysiologicalData.isPending}>Save</button>
            <button onClick={() => setIsEditing(false)} className="secondary">Cancel</button>
          </div>
        </>
      ) : (
        <>
          <dl>
            <dt>Max Heart Rate</dt>
            <dd>{athlete.physiologicalData.maxHeartRate ? `${athlete.physiologicalData.maxHeartRate} bpm` : 'Not set'}</dd>
            <dt>Lactate Threshold Heart Rate</dt>
            <dd>{athlete.physiologicalData.lactateThresholdHeartRate ? `${athlete.physiologicalData.lactateThresholdHeartRate} bpm` : 'Not set'}</dd>
            <dt>Lactate Threshold Pace</dt>
            <dd>{athlete.physiologicalData.lactateThresholdPace ? `${athlete.physiologicalData.lactateThresholdPace} min/km` : 'Not set'}</dd>
          </dl>
          <button onClick={() => setIsEditing(true)}>Edit</button>
        </>
      )}
    </div>
  );
}

function TrainingAccessSection({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  const updateTrainingAccess = useUpdateTrainingAccess();

  const handleToggle = () => {
    updateTrainingAccess.mutate({ hasTrackAccess: !athlete.trainingAccess.hasTrackAccess });
  };

  return (
    <div className="card">
      <h2>Training Access</h2>
      <div className="toggle-row">
        <span>Track Access</span>
        <label className="toggle">
          <input
            type="checkbox"
            checked={athlete.trainingAccess.hasTrackAccess}
            onChange={handleToggle}
            disabled={updateTrainingAccess.isPending}
          />
          <span className="slider"></span>
        </label>
      </div>
    </div>
  );
}

function HeartRateZonesSection({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  return (
    <div className="card">
      <h2>Heart Rate Zones</h2>
      <p className="zone-info">Calculated automatically from Lactate Threshold Heart Rate</p>
      {athlete.heartRateZones.length > 0 ? (
        <table className="zones-table">
          <thead>
            <tr>
              <th>Zone</th>
              <th>Name</th>
              <th>Min BPM</th>
              <th>Max BPM</th>
            </tr>
          </thead>
          <tbody>
            {athlete.heartRateZones.map((zone) => (
              <tr key={zone.zoneNumber}>
                <td>{zone.zoneNumber}</td>
                <td>{zone.name}</td>
                <td>{zone.minBpm}</td>
                <td>{zone.maxBpm}</td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p className="no-zones">Set your Lactate Threshold Heart Rate to calculate zones</p>
      )}
    </div>
  );
}

function PaceZonesSection({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  return (
    <div className="card">
      <h2>Pace Zones</h2>
      <p className="zone-info">Calculated automatically from Lactate Threshold Pace</p>
      {athlete.paceZones.length > 0 ? (
        <table className="zones-table">
          <thead>
            <tr>
              <th>Zone</th>
              <th>Name</th>
              <th>Min Pace</th>
              <th>Max Pace</th>
            </tr>
          </thead>
          <tbody>
            {athlete.paceZones.map((zone) => (
              <tr key={zone.zoneNumber}>
                <td>{zone.zoneNumber}</td>
                <td>{zone.name}</td>
                <td>{zone.minPace}</td>
                <td>{zone.maxPace}</td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p className="no-zones">Set your Lactate Threshold Pace to calculate zones</p>
      )}
    </div>
  );
}
