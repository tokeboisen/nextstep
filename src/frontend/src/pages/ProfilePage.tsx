import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import {
  useAthlete,
  useCreateAthlete,
  useUpdatePersonalInfo,
  useUpdatePhysiologicalData,
  useUpdateTrainingAccess,
  useUpdateHeartRateZones,
  useUpdatePaceZones,
} from '../hooks/useAthlete';
import type { HeartRateZone, PaceZone } from '../types/athlete';

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
  const [lactateThreshold, setLactateThreshold] = useState<string>(athlete.physiologicalData.lactateThreshold?.toString() || '');
  const updatePhysiologicalData = useUpdatePhysiologicalData();

  const handleSave = () => {
    updatePhysiologicalData.mutate({
      maxHeartRate: maxHeartRate ? parseInt(maxHeartRate) : null,
      lactateThreshold: lactateThreshold ? parseInt(lactateThreshold) : null,
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
            <label>Lactate Threshold (bpm)</label>
            <input type="number" value={lactateThreshold} onChange={(e) => setLactateThreshold(e.target.value)} />
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
            <dt>Lactate Threshold</dt>
            <dd>{athlete.physiologicalData.lactateThreshold ? `${athlete.physiologicalData.lactateThreshold} bpm` : 'Not set'}</dd>
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
  const [isEditing, setIsEditing] = useState(false);
  const [zones, setZones] = useState<HeartRateZone[]>(athlete.heartRateZones.length > 0 ? athlete.heartRateZones : getDefaultHeartRateZones());
  const updateHeartRateZones = useUpdateHeartRateZones();

  const handleSave = () => {
    updateHeartRateZones.mutate({ zones }, {
      onSuccess: () => setIsEditing(false),
    });
  };

  const updateZone = (index: number, field: keyof HeartRateZone, value: string | number) => {
    const newZones = [...zones];
    newZones[index] = { ...newZones[index], [field]: value };
    setZones(newZones);
  };

  return (
    <div className="card">
      <h2>Heart Rate Zones</h2>
      {isEditing ? (
        <>
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
              {zones.map((zone, index) => (
                <tr key={zone.zoneNumber}>
                  <td>{zone.zoneNumber}</td>
                  <td><input value={zone.name} onChange={(e) => updateZone(index, 'name', e.target.value)} /></td>
                  <td><input type="number" value={zone.minBpm} onChange={(e) => updateZone(index, 'minBpm', parseInt(e.target.value))} /></td>
                  <td><input type="number" value={zone.maxBpm} onChange={(e) => updateZone(index, 'maxBpm', parseInt(e.target.value))} /></td>
                </tr>
              ))}
            </tbody>
          </table>
          <div className="button-group">
            <button onClick={handleSave} disabled={updateHeartRateZones.isPending}>Save</button>
            <button onClick={() => setIsEditing(false)} className="secondary">Cancel</button>
          </div>
        </>
      ) : (
        <>
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
            <p>No heart rate zones defined</p>
          )}
          <button onClick={() => setIsEditing(true)}>Edit Zones</button>
        </>
      )}
    </div>
  );
}

function PaceZonesSection({ athlete }: { athlete: NonNullable<ReturnType<typeof useAthlete>['data']> }) {
  const [isEditing, setIsEditing] = useState(false);
  const [zones, setZones] = useState<PaceZone[]>(athlete.paceZones.length > 0 ? athlete.paceZones : getDefaultPaceZones());
  const updatePaceZones = useUpdatePaceZones();

  const handleSave = () => {
    updatePaceZones.mutate({ zones }, {
      onSuccess: () => setIsEditing(false),
    });
  };

  const updateZone = (index: number, field: keyof PaceZone, value: string | number) => {
    const newZones = [...zones];
    newZones[index] = { ...newZones[index], [field]: value };
    setZones(newZones);
  };

  return (
    <div className="card">
      <h2>Pace Zones</h2>
      {isEditing ? (
        <>
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
              {zones.map((zone, index) => (
                <tr key={zone.zoneNumber}>
                  <td>{zone.zoneNumber}</td>
                  <td><input value={zone.name} onChange={(e) => updateZone(index, 'name', e.target.value)} /></td>
                  <td><input value={zone.minPace} onChange={(e) => updateZone(index, 'minPace', e.target.value)} placeholder="mm:ss" /></td>
                  <td><input value={zone.maxPace} onChange={(e) => updateZone(index, 'maxPace', e.target.value)} placeholder="mm:ss" /></td>
                </tr>
              ))}
            </tbody>
          </table>
          <div className="button-group">
            <button onClick={handleSave} disabled={updatePaceZones.isPending}>Save</button>
            <button onClick={() => setIsEditing(false)} className="secondary">Cancel</button>
          </div>
        </>
      ) : (
        <>
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
            <p>No pace zones defined</p>
          )}
          <button onClick={() => setIsEditing(true)}>Edit Zones</button>
        </>
      )}
    </div>
  );
}

function getDefaultHeartRateZones(): HeartRateZone[] {
  return [
    { zoneNumber: 1, name: 'Recovery', minBpm: 100, maxBpm: 120 },
    { zoneNumber: 2, name: 'Aerobic', minBpm: 120, maxBpm: 140 },
    { zoneNumber: 3, name: 'Tempo', minBpm: 140, maxBpm: 160 },
    { zoneNumber: 4, name: 'Threshold', minBpm: 160, maxBpm: 175 },
    { zoneNumber: 5, name: 'VO2max', minBpm: 175, maxBpm: 190 },
  ];
}

function getDefaultPaceZones(): PaceZone[] {
  return [
    { zoneNumber: 1, name: 'Recovery', minPace: '6:30', maxPace: '7:30' },
    { zoneNumber: 2, name: 'Easy', minPace: '5:30', maxPace: '6:30' },
    { zoneNumber: 3, name: 'Tempo', minPace: '5:00', maxPace: '5:30' },
    { zoneNumber: 4, name: 'Threshold', minPace: '4:30', maxPace: '5:00' },
    { zoneNumber: 5, name: 'Interval', minPace: '4:00', maxPace: '4:30' },
  ];
}
