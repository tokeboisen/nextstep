# Claude Instructions for NextStep

## Project Overview
NextStep er en personlig webapplikation til løbeplaner. Projektet bygges inkrementelt med Domain Driven Design og CQRS pattern.

## Requirements Documentation
Al krav-dokumentation findes i `/requirements/` mappen:

- **[Functional Requirements](requirements/functional-requirements.md)** - Bounded contexts, entities, use cases
- **[Non-Functional Requirements](requirements/non-functional-requirements.md)** - Tech stack, testing, deployment, authentication
- **[Git Instructions](requirements/git-instructions.md)** - Branch strategy, commit conventions, workflow

**Læs altid disse filer for opdaterede krav før du starter en opgave.**

---

## Project Structure

```
NextStep/
├── requirements/           # Krav-dokumentation
│   ├── functional-requirements.md
│   ├── non-functional-requirements.md
│   └── git-instructions.md
├── src/                    # Al kode
│   ├── backend/            # .NET solution
│   └── frontend/           # React app
├── CLAUDE.md               # Denne fil
└── README.md               # Projekt README
```

---

## Development Guidelines

### Git Workflow
**KRITISK**: Arbejd ALDRIG direkte på `main` branch.
- Se [Git Instructions](requirements/git-instructions.md) for branch naming og commit conventions
- Opret altid en feature/bugfix branch før arbejde påbegyndes

### Architecture Principles
- Domain Driven Design med bounded contexts
- CQRS pattern - adskil commands og queries
- Én deployable unit (frontend + backend i samme container)

### Testing
- Skriv unit tests for domain logic
- Skriv integration tests for API endpoints
- Se [Non-Functional Requirements](requirements/non-functional-requirements.md) for test frameworks

---

## Common Tasks

### Før ny opgave
1. `git checkout main && git pull`
2. Læs relevante requirements filer
3. Opret ny branch med korrekt naming

### Ved implementation
1. Følg DDD og CQRS patterns
2. Skriv tests
3. Commit med beskrivende messages

### Ved afslutning
1. Sørg for alle tests passerer
2. Push branch
3. Informer om merge-readiness
