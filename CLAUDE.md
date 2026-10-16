# CLAUDE.md – NextStep

NextStep er Tokes løbetræningsprojekt: træningsplaner og -definitioner, som Claude bruger til at bygge planer og analysere Garmin-aktiviteter. Al kommunikation og alle filer er på dansk.

## Mappestruktur
- `TrainingParadigme/` – paradigme, definitioner og atletens zoner
  - `Training-paradigme.md` – overordnet paradigme (RunnersConnect-baseret): faser, principper, procedure for nye planer. **Læses først, når en plan bygges.**
  - `Training-Pyramid.md` – definition af threshold-pyramiden (threshold)
  - `Training-10-20-30.md` – definition af 10-20-30
  - `Training-20x400.md` – definition af 20 × 400 m (threshold)
  - `Training-Tempo.md` – definition af tempoløb (tempo)
  - `Training-LongRun.md` – definition af long run (long run)
  - `Training-EasyRun.md` – definition af rolige løb (easy)
  - `Training-MileTest.md` – definition af mile-test (speed-dag, opdaterer zoner)
  - `Training-5KTest.md` – definition af 5 km-test (race-uge)
  - `Athlete.md` – personlige oplysninger: fødselsdato, køn, PR'er, skader, rammer for træning
  - `Athlete-zones.md` – atletens konkrete LT-værdier, puls- og pacezoner, Garmin-zoner
- `WebApp/` – træningsprogrammer som statiske web apps
  - `DesignSystem.md` – designsystem: tokens, typografi, komponenter, kanonisk CSS og ikon
  - `ApplicationStructure.md` – applikationsstruktur: skærme, navigation, UX, datamodel, HTML-skelet, manifest, service worker og kerne-JS
  - `<Programnavn>/` – ét program pr. mappe: `index.html` (single-page HTML-app) + `manifest.webmanifest`, `sw.js` og `icons/`, så det kan installeres som app (PWA)
  - **Begge filer læses altid, før et program laves eller opdateres.**
  - Deployment: push til `main` på github.com/tokeboisen/nextstep udgiver alle `WebApp/<Programnavn>/`-mapper til https://tokeboisen.github.io/nextstep/ (`.github/workflows/pages.yml`). Kun programmerne udgives, ikke `.md`-filerne.
- `Features/` – app-features
- `Templates/` – skabeloner

## Kilder og prioritet
1. `Athlete-zones.md` – aktuelle LT-værdier og konkrete zoner. Gælder altid.
   `Athlete.md` – faste personlige oplysninger (alder beregnes fra fødselsdato).
2. `Training-paradigme.md` – hvordan planer struktureres.
3. Definitionsfiler (`Training-*.md`) – hvordan pastyper udføres og vurderes.
4. Aktuelt program – konkrete pas pr. uge.

## Konventioner
- Definitionsfiler og paradigmet indeholder **ingen konkrete tal**. Niveauer angives relativt til LT-pace/LT-puls. Konkrete tal står kun i `Athlete-zones.md`, `Athlete.md` og programmet.
- Giver en test eller threshold-pas nye LT-værdier → opdatér `Athlete-zones.md`.
- Ændres en definition → ret kun definitionsfilen og opdatér programmet, så det følger den.
- Nye pastyper, der går igen på tværs af programmer → ny fil `TrainingParadigme/Training-<navn>.md`.

## Når Claude bygger en plan
Følg proceduren i `Training-paradigme.md`. Brug pastyper fra definitionsfilerne og værdier fra `Athlete-zones.md`. Lever planen som en komplet fil med konkrete pas, pace, puls og tid.

- **Leveres som web app.** Et program bygges altid som en installérbar single-page HTML-app i `WebApp/<Programnavn>/` (`index.html` + app-filer), efter byggeproceduren i `WebApp/ApplicationStructure.md` og med CSS og ikon fra `WebApp/DesignSystem.md`. Der findes ingen template.
- **Opdatering af et program:** ret kun programindholdet (`PROGRAM` og pas-byggere); behold `PROGRAM.id`.
- Designsystem og applikationsstruktur må ikke fraviges i et program. Skal design, UI eller UX ændres, opdateres `DesignSystem.md` / `ApplicationStructure.md` først, og eksisterende programmer bygges igen.

- **Kun definerede træningspas.** Kvalitetspas vælges udelukkende fra definitionsfilerne (`TrainingParadigme/Training-*.md`). Claude må ikke selv opfinde træningspas eller bruge pastyper uden definitionsfil.
- **Speed-dage:** kun træningspas af typen Speed.
- **Threshold/tempo-dage:** kun træningspas af typen Threshold eller Tempo.
- **Long run-dage:** kun træningspas af typen Long run.
- **Easy-dage:** kun træningspas af typen Easy.
- **Pace og puls overalt:** alle pas og alle trin – også i Garmin-opsætningen – har både pace og puls (fra `Athlete-zones.md`). Feltet, der sættes som mål i Garmin, markeres; det andet er kontrol.
- Typen står øverst i hver definitionsfil (`**Type:** …`). Mangler der et egnet pas, foreslår Claude en ny definitionsfil til godkendelse i stedet for at improvisere.

## Faste regler
- Long runs under LT1; søndag er rolig aerob volumen, ikke back-to-back.
- Ved træthed skæres morgenløb først, derefter søndagsvolumen; kvalitetspas beskyttes.
- Styrke 2 × 20 min/uge er fast.
- Atlet 48 år i genopbygning: konservativ progression, flag tidlige tegn på overbelastning.
