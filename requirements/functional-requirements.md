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
  - LactateThresholdHeartRate (int, bpm)
  - LactateThresholdPace (TimeSpan, min/km)
- Zones (beregnes automatisk)
  - HeartRateZones (liste af zones med min/max bpm) - beregnes fra LactateThresholdHeartRate
  - PaceZones (liste af zones med min/max pace) - beregnes fra LactateThresholdPace
- TrainingAccess
  - HasTrackAccess (boolean) - adgang til løbebane

#### Zone Calculation

Zonerne beregnes automatisk baseret på laktattærsklen og kan ikke redigeres manuelt. Zonerne opdateres øjeblikkeligt når threshold-værdier ændres - der er ingen separat "gem" handling for zoner.

**Heart Rate Zones** (baseret på LactateThresholdHeartRate):
- Zone 1 (Recovery): < 81% af LTHR
- Zone 2 (Aerobic): 81-89% af LTHR
- Zone 3 (Tempo): 90-95% af LTHR
- Zone 4 (Threshold): 96-100% af LTHR
- Zone 5 (VO2max): > 100% af LTHR

**Pace Zones** (baseret på LactateThresholdPace):
- Zone 1 (Recovery): > 129% af LTP
- Zone 2 (Aerobic): 114-129% af LTP
- Zone 3 (Tempo): 106-113% af LTP
- Zone 4 (Threshold): 99-105% af LTP
- Zone 5 (VO2max): < 99% af LTP

#### Use Cases

**UC-AP-001: View Athlete Profile**
- Som løber vil jeg kunne se min profil med alle relevante data
- Zonerne vises som beregnet fra mine fysiologiske data

**UC-AP-002: Update Personal Info**
- Som løber vil jeg kunne opdatere mine personlige informationer (navn, fødselsdato)

**UC-AP-003: Update Physiological Data**
- Som løber vil jeg kunne opdatere mine fysiologiske data (max puls, laktattærskel puls, laktattærskel pace)
- Når fysiologiske data opdateres, genberegnes zonerne automatisk og vises øjeblikkeligt i UI
- Zonerne er dynamisk beregnede værdier - de gemmes ikke i databasen

**UC-AP-004: Update Training Access**
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
