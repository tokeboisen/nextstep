# NextStep

Tokes løbetræningsprojekt: træningsparadigme, definitioner af pastyper og træningsprogrammer som web apps.

**Live:** https://tokeboisen.github.io/nextstep/

- `TrainingParadigme/` – paradigme, definitionsfiler og atletens zoner
- `WebApp/` – designsystem, applikationsstruktur og ét program pr. mappe (`WebApp/<Programnavn>/index.html`)

## Deployment
Hvert push til `main` udgiver alle `WebApp/<Programnavn>/index.html` til GitHub Pages via `.github/workflows/pages.yml`. Kun programmerne udgives som website – ikke `.md`-filerne. Roden viderestiller til `tokeboisen/`.

## På telefonen
- **iPhone (Safari):** åbn linket → Del → Føj til hjemmeskærm.
- **Android (Chrome):** åbn linket → menu ⋮ → Føj til startskærm.

Status for gennemførte pas gemmes lokalt på hver enhed.
