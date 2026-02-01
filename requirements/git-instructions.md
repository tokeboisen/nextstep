# Git Instructions

## Branch Strategy

### Main Branch
- `main` er den primære branch
- Direkte commits til `main` er **ikke tilladt**
- Al kode skal merges via feature branches

### Branch Naming Conventions

| Type | Format | Eksempel |
|------|--------|----------|
| Feature | `feature/<beskrivelse>` | `feature/athlete-profile-api` |
| Bugfix | `bugfix/<beskrivelse>` | `bugfix/login-validation` |
| Hotfix | `hotfix/<beskrivelse>` | `hotfix/critical-auth-fix` |
| Refactor | `refactor/<beskrivelse>` | `refactor/cqrs-handlers` |
| Docs | `docs/<beskrivelse>` | `docs/api-documentation` |
| Test | `test/<beskrivelse>` | `test/integration-tests` |

### Naming Guidelines
- Brug lowercase
- Brug bindestreger (`-`) som separator
- Hold navne korte men beskrivende
- Inkluder issue nummer hvis relevant: `feature/123-athlete-zones`

---

## Commit Messages

### Format
```
<type>: <kort beskrivelse>

[Valgfri længere beskrivelse]
```

### Types
- `feat`: Ny funktionalitet
- `fix`: Bug fix
- `refactor`: Kode refaktorering (ingen ny funktionalitet)
- `test`: Tilføjelse eller rettelse af tests
- `docs`: Dokumentation
- `chore`: Maintenance opgaver (dependencies, config)
- `style`: Formatering (ingen kode ændringer)

### Eksempler
```
feat: add athlete profile entity

fix: correct heart rate zone validation

refactor: extract zone calculation to separate service

test: add unit tests for athlete repository

docs: update API documentation for zones endpoint
```

---

## Workflow

### 1. Start ny opgave
```bash
git checkout main
git pull
git checkout -b feature/<beskrivelse>
```

### 2. Arbejd på opgaven
- Commit ofte med beskrivende messages
- Hold commits fokuserede (én ting per commit)

### 3. Afslut opgaven
```bash
git push -u origin <branch-name>
```

### 4. Merge til main
- Opret Pull Request (hvis relevant)
- Eller merge lokalt:
```bash
git checkout main
git merge <branch-name>
git push
```

---

## Claude Code Instructions

**VIGTIGT**: Claude må kun arbejde på git branches - aldrig direkte på `main`.

1. Ved start af ny opgave: Opret altid en passende branch først
2. Følg branch naming conventions
3. Commit med beskrivende messages
4. Push branch når arbejdet er færdigt
