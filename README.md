# NextStep

Tokes løbetræningsprojekt: træningsparadigme, definitioner af pastyper og træningsprogrammer som web apps.

**Live:** https://tokeboisen.github.io/nextstep/

- `TrainingParadigme/` – paradigme, definitionsfiler og atletens zoner
- `WebApp/` – designsystem, applikationsstruktur og ét program pr. mappe (`WebApp/<Programnavn>/`: `index.html`, manifest, service worker og ikoner)

## Deployment
Hvert push til `main` udgiver alle `WebApp/<Programnavn>/`-mapper til GitHub Pages via `.github/workflows/pages.yml`. Kun programmerne udgives som website – ikke `.md`-filerne. Roden viderestiller til `tokeboisen/`.

## På telefonen
Programmet er en installérbar web app (PWA) – gratis og uden app-butik. Den åbner i fuld skærm, virker offline efter første besøg og henter en ny version i baggrunden (vises ved næste åbning).

- **iPhone (Safari):** åbn https://tokeboisen.github.io/nextstep/tokeboisen/ → Del → Føj til hjemmeskærm.
- **Android (Chrome):** åbn linket → "Installer app" (eller menu ⋮ → Installer app).

Status for gennemførte pas gemmes lokalt på hver enhed.
