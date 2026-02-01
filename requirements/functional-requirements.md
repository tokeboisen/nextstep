# Functional Requirements

## Overview
NextStep er en webapplikation til at lave løbeplaner. Applikationen er personlig og bruges kun af én bruger.

## Architecture
- Domain Driven Design med adskilte bounded contexts
- CQRS pattern (Command Query Responsibility Segregation)

---

## Bounded Contexts

### 1. Athlete Profile Context

Formål: Håndtering af løberens profil og fysiologiske data, som er relevante for at generere løbeplaner.

#### Entities

**Athlete**
- PersonalInfo
  - Name (string, required)
  - BirthDate (date, required) - bruges til at beregne alder
- PhysiologicalData
  - MaxHeartRate (int, bpm)
  - LactateThreshold (int, bpm)
- Zones
  - HeartRateZones (liste af zones med min/max bpm)
  - PaceZones (liste af zones med min/max pace)
- TrainingAccess
  - HasTrackAccess (boolean) - adgang til løbebane

#### Use Cases

**UC-AP-001: View Athlete Profile**
- Som løber vil jeg kunne se min profil med alle relevante data

**UC-AP-002: Update Personal Info**
- Som løber vil jeg kunne opdatere mine personlige informationer (navn, fødselsdato)

**UC-AP-003: Update Physiological Data**
- Som løber vil jeg kunne opdatere mine fysiologiske data (max puls, laktattærskel)

**UC-AP-004: Manage Heart Rate Zones**
- Som løber vil jeg kunne definere mine pulszoner

**UC-AP-005: Manage Pace Zones**
- Som løber vil jeg kunne definere mine pace-zoner

**UC-AP-006: Update Training Access**
- Som løber vil jeg kunne angive om jeg har adgang til løbebane

---

## Future Contexts (ikke i scope for v1)

### Training Plan Context
- Generering af løbeplaner baseret på athlete profile
- Periodisering
- Træningspas

### Workout Context
- Registrering af gennemførte træningspas
- Integration med Garmin/Strava
