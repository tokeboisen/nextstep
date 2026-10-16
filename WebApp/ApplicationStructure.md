# NextStep applikationsstruktur

**Denne fil beskriver hele appens UI-struktur og UX.** Claude bygger altid et træningsprogram ud fra denne fil og `DesignSystem.md` – der findes ingen template. Designsystemet ejer det visuelle (CSS, ikon); denne fil ejer skærme, navigation, adfærd, datamodel og kode.

## 1. Arkitektur
- **Single-page HTML-app i én fil:** `WebApp/<Programnavn>/index.html`. Ingen andre filer, ingen build-step, ingen frameworks.
- Al CSS (fra `DesignSystem.md`), al JS (kernen nedenfor) og alt programindhold ligger inline.
- Eneste eksterne ressource er Google Fonts. Uden net vises systemfonte.
- Navigation sker med hash-routes i samme side; ingen sideskift.
- Tilstand (gennemførte pas, senest viste uge) gemmes i `localStorage` på enheden.
- Kan føjes til hjemmeskærmen fra browserens menu (ikon og `theme-color` er indlejret).

Appen består af tre lag:

| Lag | Kilde | Ændres pr. program? |
|---|---|---|
| Udseende | CSS og ikon fra `DesignSystem.md` | Nej |
| Kerne | HTML-skelet og kerne-JS i denne fil (afsnit 9–10) | Nej |
| Programindhold | `PROGRAM`-objektet (afsnit 7) | Ja – det eneste, der skrives pr. program |

## 2. Informationsarkitektur

```
Bundmenu
├── I dag            #/
│   ├── Dagens pas                         (pas-skærm med kicker "I dag")
│   ├── Hviledag / før start → Næste pas   (besked + pas-skærm med kicker "Næste pas")
│   └── Program slut                       (besked)
└── Program          #/uge/<n>
    ├── Ugestribe (alle uger) + ugenavigation ‹ ›
    └── 7 dagkort → Pas  #/pas/<uge>/<dag>
                         └── ‹ Uge n (tilbage)
```

| Route | Skærm | Aktiv fane |
|---|---|---|
| `#/` (eller tom) | I dag | I dag |
| `#/uge/<n>` | Uge n (begrænses til 1…antal uger) | Program |
| `#/pas/<uge>/<dag>` | Pas (dag 0 = mandag … 6 = søndag) | Program |
| Ukendt route eller dag uden pas | I dag | I dag |

`?dato=YYYY-MM-DD` i URL'en simulerer dagens dato (til test og gennemsyn).

## 3. Skærme

### 3.1 I dag (`#/`)
Formål: Åbn appen → se straks, hvad der skal løbes i dag.

| Tilstand | Visning |
|---|---|
| Dag med pas | Pas-skærmen for i dag med kicker "I dag – <Dag> <dato> · Uge n" |
| Hviledag i programmet | Besked "<Dag dato>: hvile" + pas-skærmen for næste pas (morgenløb springes over) med kicker "Næste pas" |
| Før programstart | Besked "<Dag dato>: før start" med startdato + første pas |
| Efter programmets sidste dag | Hero "Programmet er gennemført" + besked fra `PROGRAM.end` |

### 3.2 Uge (`#/uge/<n>`)
Formål: Overblik over en uge og hurtig adgang til hvert pas.

Rækkefølge:
1. **Hero** – kicker "Blok b: <bloknavn>", h1 "Uge n" (+ " – deload" / " – race"), meta-tal: ugevolumen (`~km`), løbetid ekskl. morgenløb (`t m`), antal morgenløb, gennemført (`x/y`)
2. **Ugestribe** – én knap pr. uge; valgt uge udfyldt, aktuel uge med prik, deload-uger stiplet; valgt uge scrolles til midten
3. **Ugenavigation** – ‹ forrige · datointerval (man–søn) + undertitel (`label` eller bloknavn) · næste ›; deaktiveret i første/sidste uge
4. **7 dagkort** (man–søn) – pastypefarve som venstrekant, titel (+ styrke-tag), opsummering, tid, "✓ Klaret" når gennemført; dagen i dag har ring; dage uden pas vises som "Hvile" og kan ikke klikkes
5. **Fodnote** – `PROGRAM.footer` (fx træthedsregler)

Den viste uge huskes og bruges, når man trykker "Program" i bundmenuen. Første gang vises den aktuelle uge.

### 3.3 Pas (`#/pas/<uge>/<dag>` og i "I dag")
Formål: Alt, der skal til for at sætte passet op i Garmin og løbe det.

Rækkefølge:
1. **Hero** med pastypefarvet bjælke – "‹ Uge n" (kun via route), kicker "<Dag> <dato> · Uge n", h1 = titel, chips (pastype, niveau, styrke, deload), meta-tal: totaltid (eller måltid for race), pace, puls, antal Garmin-trin, "+20 min styrke" når relevant
2. **Opsummering** – én linje (`OP + hovedsæt med pace og puls + NL`)
3. **Formål** – fra definitionsfilen
4. **Splits** – kun hvis passet har mellemtider
5. **Garmin-opsætning** – nummererede trin med type, varighed, **pace og puls** (Garmin-målet markeret) og note; gentagelser som stiplet blok "Gentag n× – note" med indlejrede trin; evt. Garmin-note under kortet
6. **Styring** – regler fra definitionsfilen
7. **Styrke A/B · 20 min** – kun når passet har styrke: belastning, øvelsestabel (øvelse, sæt × reps, fokus), noter
8. **Gennemført-knap** – skifter mellem "Markér som gennemført" og "✓ Gennemført" (grøn)

## 4. UX-regler
- **I dag først:** appen åbner altid på dagens pas; ingen forsider eller menuer at klikke sig igennem.
- **Én hånd:** primær navigation i bunden; store trykflader (dagkort i fuld bredde, knapper ≥ 2.6rem).
- **Garmin-tro:** trinene vises præcis, som de tastes ind i Garmin (type, varighed, pace, puls, note). Garmin understøtter ikke gentagelser i gentagelser – brug én gentagelse pr. blok.
- **Altid pace og puls:** hvert pas og hvert trin – også i Garmin-opsætningen – viser både pace og puls. Det felt, der sættes som mål i Garmin, markeres "Garmin-mål"; det andet er kontrol. Er pace eller puls ikke et mål (fx sprint, korte stykker eller gang), angives det forventede niveau i tekst.
- **Ét klik til et pas:** fra en uge er alle pas ét tryk væk; tilbage-knappen går tilbage i historikken.
- **Status uden login:** "gennemført" gemmes lokalt pr. enhed; fejl i `localStorage` ignoreres stille.
- **Ro i hvilen:** hviledage er tydelige, men ikke klikbare; morgenløb tæller ikke med i løbetid og vises aldrig som "næste pas".
- **Skærmskift:** siden scrolles til toppen; kort fade, hvis brugeren ikke har slået bevægelse fra.
- **Tilgængelighed:** knapper har `aria-label` hvor teksten ikke siger det hele (‹ › og ugeknapper); synlig fokusmarkering; tekst og farver følger designsystemets kontrast i lys og mørk.

## 5. Navigation og tilstand

| Handling | Resultat |
|---|---|
| Tryk "I dag" | `#/` |
| Tryk "Program" | `#/uge/<senest viste uge>` (ellers aktuel uge) |
| Tryk ugeknap / ‹ › | `#/uge/<n>` |
| Tryk dagkort | `#/pas/<uge>/<dag>` |
| Tryk "‹ Uge n" | Tilbage i historikken (eller `#/uge/n`, hvis der ingen historik er) |
| Tryk gennemført-knap | Skifter status og tegner skærmen igen |

| Nøgle i `localStorage` | Værdi |
|---|---|
| `<id>-done-<uge>-<dag>` | `"1"` når passet er gennemført |
| `<id>-week` | Senest viste uge |

`<id>` er `PROGRAM.id`, så flere programmer på samme enhed ikke deler status.

## 6. Datamodel

### Program
```js
const PROGRAM = {
  id: 'mile-2026',              // unik, bruges i localStorage-nøgler
  title: 'Mile 12 uger',        // sidens <title>
  start: '2026-09-28',          // mandag i uge 1 (YYYY-MM-DD)
  blocks: {1:'Genopbygning', 2:'Opbygning', 3:'Specifik og skarp'},
  weeks: [ /* Uge-objekter, uge 1 først */ ],
  strength: (type, week) => ({name, load, ex, notes}),   // valgfri
  footer: 'Tekst under ugeoversigten',
  end: {title: 'Næste skridt', text: '…'}
};
```

### Uge
```js
{ n: 1, b: 1, km: 27, dl: false, race: false, label: 'Valgfri undertitel',
  days: [man, tir, ons, tor, fre, lør, søn] }   // hver: Pas, funktion der returnerer Pas, eller null (hvile)
```

### Pas
```js
{
  kind,      // 'speed' | 'thr' | 'tempo' | 'long' | 'easy' | 'morning' | 'test' | 'race'
  title,     // fx '3 × 10-20-30'
  time,      // totaltid i minutter (null for race – brug timeLabel)
  timeLabel, // valgfri tekst i stedet for time, fx '5:45'
  pace,      // hovedsættets pace, fx '4:40' eller '5:30–5:50' (påkrævet)
  hr,        // hovedsættets puls, fx '159–168' eller '<140' (påkrævet)
  sum,       // én linje: OP + hovedsæt med pace og puls + NL
  purpose,   // fra definitionsfilens Formål
  steps,     // Garmin-trin: S() og R()
  rules,     // fra definitionsfilens Styring
  level,     // valgfri: fx 'Sub-threshold'
  strength,  // 'A' | 'B' | null
  garmin,    // valgfri note under Garmin-opsætning
  split      // valgfri: {label, rows: [['400 m','1:33'], …]} – 4 rækker
}
```
`kind` følger definitionsfilens `**Type:**` (Speed → `speed`, Threshold → `thr`, Tempo → `tempo`, Long run → `long`, Easy → `easy`).

### Trin
- `S(kind, dur, pace, hr, note, tgt)` – ét trin
  - `kind`: `warm` | `run` | `rec` | `rest` | `cool`
  - `dur`: tid (`'15:00'`) eller distance (`'400 m'`)
  - `pace`: fx `'4:40'`, `'5:30–5:50'` eller tekst (`'Gang/stå'`) – påkrævet
  - `hr`: fx `'159–168'`, `'<140'` eller tekst (`'Styres ikke på 20″'`) – påkrævet
  - `tgt`: Garmin-mål – `'pace'` (standard), `'hr'` eller `'none'`; ved `'pace'` og en tal-pace vises Garmin-interval automatisk
- `R(n, steps, note)` – gentag `steps` n gange; må ikke indeholde en anden `R()`
- `g(pace)` – Garmin-interval for en pace: `'4:30'` → `'4:25–4:35'`, `'4:40–4:45'` → `'4:38–4:47'`

### Styrke
`PROGRAM.strength(type, uge)` returnerer `{name: 'Styrke A', load: 'tekst', ex: [[øvelse, 'sæt × reps', fokus], …], notes: ['…']}`.

## 7. Programindhold – sådan skrives det
1. Ugestrukturen følger `TrainingParadigme/Athlete.md`; alle pas undtagen morgenløb kommer **kun** fra definitionsfilerne (`Training-*.md`).
2. Skriv én pas-bygger pr. definitionsfil (fx `t1020(...)`, `pyr(...)`, `tempo(...)`, `r400(...)`, `longRun(...)`, `easy(...)`) plus en bygger til morgenløb. `purpose`, `rules`, struktur og varianter tages fra definitionsfilen; pace og puls fra `Athlete-zones.md`.
3. Udfyld `PROGRAM.weeks` med en byggerkald pr. dag.
4. Al tekst på dansk; ingen henvisninger til filer, der ikke findes.

Eksempel (forkortet – to uger):

```js
/* ============ Programindhold ============ */
const OP=()=>[S('warm','15:00','5:45–6:15','130–145','Rolig start','hr')];
const NL=(m=10)=>[S('cool',m+':00','6:00–6:30','<135','Rolig','hr')];
const STRIDES=n=>R(n,[S('run','0:20','3:45','Styres ikke – stiger kort','Stride – hurtigt og afslappet, ikke sprint'),
  S('rec','1:00','Gang/let jog','<130','','none')],'Strides');

function t1020(blocks,p20,time){            // Training-10-20-30.md
  const steps=[...OP()];
  for(let i=0;i<blocks;i++){
    steps.push(R(5,[S('rec','0:30','6:30–7:00','Styres ikke på 30″','Let jog (≈30 %)'),
      S('run','0:20',p20,'Styres ikke på 20″','Moderat (≈60 %)'),
      S('run','0:10','<4:20 – næsten maks','Over LT-puls (>169)','Næsten maksimal sprint','none')],'Blok '+(i+1)));
    if(i<blocks-1) steps.push(S('rest','2:00','Gang/stå','<130','Gang eller stående hvile','none'));
  }
  steps.push(...NL());
  return {kind:'speed',title:blocks+' × 10-20-30',time,strength:'A',pace:p20,hr:'Op mod 169',
    sum:`OP + ${blocks}×10-20-30 (20″ @${p20}), puls op mod 169, 2′ gang + NL`,
    purpose:'Hæve VO2max og løbsøkonomi med kort, hård intensitet over LT2.',
    steps,rules:['10″ styres på følelse – GPS-pace er upålidelig på 10″.','Pulsen må nærme sig LT-puls i sidste blok.']};
}
function tempo(min,pace,time){              // Training-Tempo.md
  return {kind:'tempo',title:`Tempo ${min} min`,time,level:'Tempo',strength:'B',pace,hr:'159–168',
    sum:`OP + ${min} min @${pace}, puls 159–168 + NL`,
    purpose:'Hæve LT2 og evnen til at holde et højt, kontrolleret tempo uden pauser.',
    steps:[...OP(),S('run',min+':00',pace,'159–168 (Z4, under LT-puls)','Jævnt tempo'),...NL()],
    rules:['Pace styrer, puls kontrollerer.','Når pulsen LT-puls, sænkes tempoet 5″/km.']};
}
function longRun(km,time){                  // Training-LongRun.md
  return {kind:'long',title:`Long run ${km} km`,time,pace:'5:30–5:50',hr:'135–145',
    sum:`${km} km @5:30–5:50, puls 135–145 + 4×20″ strides`,
    purpose:'Aerob basis under LT1 og gradvis opbygning af volumen.',
    steps:[S('run',km+' km','5:30–5:50','135–145','Pulsen styrer','hr'),STRIDES(4)],
    rules:['Pulsen styrer, ikke pacen.','Cardiac drift over ~5 % i anden halvdel = distancen er i overkanten.']};
}
function easy(km,time){                     // Training-EasyRun.md
  return {kind:'easy',title:`Rolig ${km} km`,time,pace:'5:45–6:00',hr:'<140',
    sum:`${km} km @5:45–6:00, puls <140 + 6×20″ strides`,
    purpose:'Rolig aerob volumen under LT1.',
    steps:[S('run',km+' km','5:45–6:00','<140','Pulsen styrer','hr'),STRIDES(6)],
    rules:['Pulsen styrer, ikke pacen.','Strides skal føles lette og hurtige – ikke udmattende.']};
}
const MORNING={kind:'morning',title:'Morgenløb (valgfrit)',time:15,pace:'6:00–6:30',hr:'<135',
  sum:'15 min @6:00–6:30, puls <135',
  purpose:'Valgfri ekstra aerob volumen.',steps:[S('run','15:00','6:00–6:30','<135','Pulsen styrer','hr')],
  rules:['Det første, der skæres ved træthed.']};

const PROGRAM={
  id:'eksempel',title:'Eksempelprogram',start:'2026-10-12',
  blocks:{1:'Genopbygning'},
  weeks:[
    {n:1,b:1,km:27,days:[MORNING,()=>t1020(3,'5:00–5:15',49),()=>easy(5,29),()=>tempo(15,'4:40',45),MORNING,()=>longRun(8,45),()=>easy(4,24)]},
    {n:2,b:1,km:30,dl:true,days:[null,()=>t1020(2,'5:00–5:15',42),()=>easy(5,29),()=>tempo(10,'4:40',40),null,()=>longRun(8,45),()=>easy(4,24)]}
  ],
  strength:(t,w)=>({name:'Styrke '+t,load:w===1?'Kropsvægt':'Let vægt',
    ex:t==='A'?[['Excentriske tåhævninger','3 × 12','Achilles']]:[['Step-ups','3 × 10/ben','Afsæt']],
    notes:['Ømhed højst 3/10 og væk inden næste kvalitetspas.']}),
  footer:'Ved træthed skæres morgenløb først, derefter søndagsvolumen.',
  end:{title:'Næste skridt',text:'Planlæg næste blok ud fra Training-paradigme.md.'}
};
```

## 8. Byggeprocedure
1. Læs `DesignSystem.md` og denne fil.
2. Opret `WebApp/<Programnavn>/index.html` ud fra HTML-skelettet (afsnit 9):
   - `{{CSS}}` ← CSS-blokken fra `DesignSystem.md`, uændret
   - `{{IKON}}` ← SVG'en fra `DesignSystem.md`, uændret (som JS-streng)
   - `{{KERNE}}` ← kerne-JS (afsnit 10), uændret
   - `{{PROGRAM}}` ← programindholdet (afsnit 7)
3. **Opdatering:** ret kun `{{PROGRAM}}`-delen. Ændres `PROGRAM.id`, nulstilles "gennemført" – behold id'et ved opdateringer af samme program.
4. Kør tjeklisten (afsnit 11).

## 9. HTML-skelet (kanonisk)

```html
<!DOCTYPE html>
<html lang="da">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover">
<title>NextStep</title>
<meta name="theme-color" content="#16232F">
<meta name="mobile-web-app-capable" content="yes">
<meta name="apple-mobile-web-app-capable" content="yes">
<meta name="apple-mobile-web-app-title" content="NextStep">
<link rel="icon" id="icon">
<link rel="apple-touch-icon" id="touch-icon">
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@500;700&family=Barlow:wght@400;500;600&display=swap" rel="stylesheet">
<style>
{{CSS}}
</style>
</head>
<body>
<div id="app"></div>
<nav class="tabs" aria-label="Hovedmenu"><div class="wrap">
  <button class="tab" id="tab-today" onclick="go('#/')">I dag</button>
  <button class="tab" id="tab-prog" onclick="go('#/uge/'+lastWeek())">Program</button>
</div></nav>
<script>
const ICON=`{{IKON}}`;
{{KERNE}}
{{PROGRAM}}
start();
</script>
</body>
</html>
```

## 10. Kerne-JS (kanonisk)

```js
/* ============ Kerne – ændres ikke pr. program ============ */
const DAYS=['Mandag','Tirsdag','Onsdag','Torsdag','Fredag','Lørdag','Søndag'];
const SD=['Man','Tir','Ons','Tor','Fre','Lør','Søn'];
const MON=['januar','februar','marts','april','maj','juni','juli','august','september','oktober','november','december'];
let START, NW;

/* Kalender */
function dateOf(w,d){const x=new Date(START);x.setDate(x.getDate()+(w-1)*7+d);return x}
function dShort(x){return x.getDate()+'/'+(x.getMonth()+1)}
function dLong(x){return x.getDate()+'. '+MON[x.getMonth()]}
function todayDate(){
  const q=new URLSearchParams(location.search).get('dato');
  const t=q?new Date(q+'T12:00:00'):new Date();
  return new Date(t.getFullYear(),t.getMonth(),t.getDate());
}
function dayIndex(){return Math.round((todayDate()-START)/86400000)}

/* Pace */
function toS(p){const [m,s]=p.split(':').map(Number);return m*60+s}
function fmt(s){return Math.floor(s/60)+':'+String(s%60).padStart(2,'0')}
function g(p){
  if(p.includes('–')){const [a,b]=p.split('–');return fmt(toS(a)-2)+'–'+fmt(toS(b)+2)}
  return fmt(toS(p)-5)+'–'+fmt(toS(p)+5);
}

/* Trin */
const S=(kind,dur,pace,hr,note,tgt='pace')=>({kind,dur,pace,hr,note,tgt});
const PACE_RE=/^\d+:\d\d(–\d+:\d\d)?$/;
const R=(n,steps,note)=>({repeat:n,steps,note});
const KIND={warm:'Opvarmning',run:'Løb',rec:'Restitution',rest:'Hvile',cool:'Nedløb'};
const COL={speed:'var(--speed)',thr:'var(--thr)',tempo:'var(--tempo)',long:'var(--long)',easy:'var(--easy)',test:'var(--test)',race:'var(--test)',morning:'var(--morning)'};
const TYPE={speed:'Speed',thr:'Threshold',tempo:'Tempo',long:'Long run',easy:'Rolig',test:'Testløb',race:'Race',morning:'Morgenløb'};

/* Program-opslag */
function session(wn,d){
  const w=PROGRAM.weeks[wn-1], f=w&&w.days[d];
  if(!f) return null;
  const s=typeof f==='function'?f():{...f};
  if(s.strength&&PROGRAM.strength) s.str=PROGRAM.strength(s.strength,wn);
  return s;
}
function mornings(wn){let n=0;for(let d=0;d<7;d++){const s=session(wn,d);if(s&&s.kind==='morning')n++}return n}

/* Lager */
const key=k=>PROGRAM.id+'-'+k;
function isDone(w,d){try{return localStorage.getItem(key(`done-${w}-${d}`))==='1'}catch(e){return false}}
function setDone(w,d,v){try{v?localStorage.setItem(key(`done-${w}-${d}`),'1'):localStorage.removeItem(key(`done-${w}-${d}`))}catch(e){}}
function lastWeek(){try{const v=+localStorage.getItem(key('week'));if(v>=1&&v<=NW)return v}catch(e){} return Math.min(NW,Math.max(1,Math.floor(dayIndex()/7)+1))}
function saveWeek(n){try{localStorage.setItem(key('week'),n)}catch(e){}}

/* Rendering */
const $=id=>document.getElementById(id);
const esc=s=>String(s).replace(/[&<>"]/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;'}[c]));
function go(h){location.hash=h}
function tLabel(s){return s.timeLabel||(s.time+' min')}
function countSteps(steps){return steps.reduce((a,s)=>a+(s.repeat?s.steps.length:1),0)}
function toggleDone(w,d){setDone(w,d,!isDone(w,d));route()}

const GT='<span class="tag">Garmin-mål</span>';
function stepHTML(st,num){
  if(st.repeat){
    return `<li class="step"><div class="num">${num}</div><div><div class="rep">
      <div class="rephead"><span>Gentag ${st.repeat}×${st.note?' – '+esc(st.note):''}</span></div>
      <ol class="steps">${st.steps.map(x=>stepHTML(x,'')).join('')}</ol></div></div></li>`;
  }
  const inner=`<div class="srow"><span class="stype k-${st.kind}"><i></i>${KIND[st.kind]}</span><span class="sdur">${esc(st.dur)}</span></div>
    <div class="stgt">Pace ${esc(st.pace)}${st.tgt==='pace'&&PACE_RE.test(st.pace)?` (Garmin ${g(st.pace)})`:''}${st.tgt==='pace'?GT:''}</div>
    <div class="stgt">Puls ${esc(st.hr)}${st.tgt==='hr'?GT:''}</div>${st.note?`<div class="snote">${esc(st.note)}</div>`:''}`;
  return num===''?`<li class="step"><div>${inner}</div></li>`:`<li class="step"><div class="num">${num}</div><div>${inner}</div></li>`;
}

function detailHTML(wn,d,opts={}){
  const s=session(wn,d), date=dateOf(wn,d), w=PROGRAM.weeks[wn-1], done=isDone(wn,d);
  const chips=[TYPE[s.kind]]; if(s.level)chips.push(s.level); if(s.str)chips.push(s.str.name); if(w.dl)chips.push('Deload');
  let h=`<header class="hero" style="--c:${COL[s.kind]}"><div class="bar"></div><div class="wrap">
    ${opts.back?`<button class="back" onclick="history.length>1?history.back():go('#/uge/${wn}')">‹ Uge ${wn}</button>`:''}
    <p class="kicker">${opts.kicker?esc(opts.kicker)+' – ':''}${DAYS[d]} ${dLong(date)} · Uge ${wn}</p>
    <h1>${esc(s.title)}</h1>
    <ul class="chips">${chips.map(c=>`<li>${esc(c)}</li>`).join('')}</ul>
    <div class="meta"><div><b>${esc(tLabel(s))}</b><span>${s.kind==='race'?'måltid':'totaltid'}</span></div>
    <div><b>${esc(s.pace)}</b><span>pace</span></div>
    <div><b>${esc(s.hr)}</b><span>puls</span></div>
    <div><b>${countSteps(s.steps)}</b><span>Garmin-trin</span></div>
    ${s.str?`<div><b>+20 min</b><span>${esc(s.str.name.toLowerCase())}</span></div>`:''}</div>
  </div></header><div class="wrap">`;
  if(opts.notice)h+=opts.notice;
  h+=`<p class="muted" style="margin-top:1rem">${esc(s.sum)}</p>`;
  h+=`<h2>Formål</h2><p>${esc(s.purpose)}</p>`;
  if(s.split)h+=`<h2>${esc(s.split.label)}</h2><div class="splits">${s.split.rows.map(r=>`<div><b>${esc(r[1])}</b><span>${esc(r[0])}</span></div>`).join('')}</div>`;
  h+=`<h2>Garmin-opsætning</h2><div class="card"><ol class="steps">${s.steps.map((x,i)=>stepHTML(x,i+1)).join('')}</ol></div>`;
  if(s.garmin)h+=`<p class="muted" style="font-size:.88rem">${esc(s.garmin)}</p>`;
  if(s.rules)h+=`<h2>Styring</h2><ul class="rules">${s.rules.map(r=>`<li>${esc(r)}</li>`).join('')}</ul>`;
  if(s.str){
    h+=`<h2>${esc(s.str.name)} · 20 min</h2><p class="muted">${esc(s.str.load)}</p>
    <div class="card tablewrap"><table><tr><th>Øvelse</th><th>Sæt × reps</th><th>Fokus</th></tr>
    ${s.str.ex.map(e=>`<tr><td>${esc(e[0])}</td><td class="n">${esc(e[1])}</td><td class="muted">${esc(e[2])}</td></tr>`).join('')}</table></div>`;
    if(s.str.notes&&s.str.notes.length)h+=`<ul class="rules" style="margin-top:.7rem">${s.str.notes.map(n=>`<li>${esc(n)}</li>`).join('')}</ul>`;
  }
  h+=`<button class="donebtn ${done?'is':''}" onclick="toggleDone(${wn},${d})">${done?'✓ Gennemført':'Markér som gennemført'}</button></div>`;
  return h;
}

function weekHTML(wn){
  saveWeek(wn);
  const w=PROGRAM.weeks[wn-1], ti=dayIndex(), cw=Math.floor(ti/7)+1;
  let total=0,cnt=0,dn=0;
  for(let d=0;d<7;d++){const s=session(wn,d);if(s){cnt++;if(s.time&&s.kind!=='morning')total+=s.time;if(isDone(wn,d))dn++}}
  let h=`<header class="hero"><div class="wrap">
    <p class="kicker">Blok ${w.b}: ${esc(PROGRAM.blocks[w.b])}</p>
    <h1>Uge ${wn}${w.dl?' – deload':''}${w.race?' – race':''}</h1>
    <div class="meta"><div><b>~${w.km} km</b><span>ugevolumen</span></div>
    <div><b>${Math.floor(total/60)} t ${total%60} m</b><span>løbetid ekskl. morgenløb</span></div>
    <div><b>${mornings(wn)}</b><span>morgenløb</span></div>
    <div><b>${dn}/${cnt}</b><span>gennemført</span></div></div>
  </div></header><div class="wrap">
  <div class="weekstrip" role="tablist">${PROGRAM.weeks.map(x=>`<button class="wk ${x.n===wn?'on':''} ${x.n===cw?'now':''} ${x.dl?'dl':''}" onclick="go('#/uge/${x.n}')" aria-label="Uge ${x.n}">${x.n}</button>`).join('')}</div>
  <div class="weeknav"><button class="navbtn" ${wn===1?'disabled':''} onclick="go('#/uge/${wn-1}')" aria-label="Forrige uge">‹</button>
    <div class="weektitle"><b>${dShort(dateOf(wn,0))} – ${dShort(dateOf(wn,6))}</b><span>${esc(w.label||PROGRAM.blocks[w.b])}</span></div>
    <button class="navbtn" ${wn===NW?'disabled':''} onclick="go('#/uge/${wn+1}')" aria-label="Næste uge">›</button></div>
  <ul class="days">`;
  for(let d=0;d<7;d++){
    const s=session(wn,d), date=dateOf(wn,d), isT=(wn-1)*7+d===ti;
    if(!s){h+=`<li><div class="daycard rest ${isT?'today':''}"><div class="d"><b>${SD[d]}</b><span>${dShort(date)}</span></div><div><h3 class="muted" style="font-weight:500">Hvile</h3></div><div></div></div></li>`;continue}
    const done=isDone(wn,d);
    h+=`<li><button class="daycard ${isT?'today':''}" style="--c:${COL[s.kind]}" onclick="go('#/pas/${wn}/${d}')">
      <div class="d"><b>${SD[d]}</b><span>${dShort(date)}</span></div>
      <div><h3>${esc(s.title)}${s.str?`<span class="tag">${esc(s.str.name)}</span>`:''}</h3><p class="sum">${esc(s.sum)}</p></div>
      <div class="t">${esc(tLabel(s))}${done?'<small>✓ Klaret</small>':''}</div></button></li>`;
  }
  h+=`</ul>${PROGRAM.footer?`<p class="muted" style="font-size:.85rem;margin-top:1rem">${esc(PROGRAM.footer)}</p>`:''}</div>`;
  return h;
}

function nextSession(from){
  for(let i=Math.max(0,from);i<NW*7;i++){const w=Math.floor(i/7)+1,d=i%7,s=session(w,d);if(s&&s.kind!=='morning')return [w,d]}
  return null;
}
function todayHTML(){
  const i=dayIndex(), t=todayDate(), label=`${DAYS[(t.getDay()+6)%7]} ${dLong(t)}`;
  if(i>=0&&i<NW*7){const w=Math.floor(i/7)+1,d=i%7;if(session(w,d))return detailHTML(w,d,{kicker:'I dag'})}
  const nx=i<NW*7?nextSession(i+1):null;
  if(!nx){
    const e=PROGRAM.end||{};
    return `<header class="hero"><div class="wrap"><p class="kicker">${label}</p><h1>Programmet er gennemført</h1></div></header><div class="wrap">${e.title?`<div class="notice"><h3>${esc(e.title)}</h3><p>${esc(e.text||'')}</p></div>`:''}</div>`;
  }
  const msg=i<0?`Programmet starter mandag ${dLong(START)}. Første pas er herunder.`:'Hviledag. Restitution er en del af træningen – næste pas er herunder.';
  return detailHTML(nx[0],nx[1],{kicker:'Næste pas',notice:`<div class="notice"><h3>${label}: ${i<0?'før start':'hvile'}</h3><p class="muted">${msg}</p></div>`});
}

function route(){
  const h=location.hash||'#/';
  const m=h.match(/^#\/(uge|pas)\/(\d+)(?:\/(\d))?/);
  let html, prog=false;
  if(m&&m[1]==='uge'){html=weekHTML(Math.min(NW,Math.max(1,+m[2])));prog=true}
  else if(m&&m[1]==='pas'&&m[3]!==undefined&&+m[2]>=1&&+m[2]<=NW&&session(+m[2],+m[3])){html=detailHTML(+m[2],+m[3],{back:true});prog=true}
  else html=todayHTML();
  $('tab-today').classList.toggle('on',!prog);
  $('tab-prog').classList.toggle('on',prog);
  $('app').innerHTML=`<main class="view">${html}</main>`;
  window.scrollTo(0,0);
  const on=document.querySelector('.wk.on'); if(on) on.scrollIntoView({inline:'center',block:'nearest'});
}

function start(){
  START=new Date(PROGRAM.start+'T00:00:00');
  NW=PROGRAM.weeks.length;
  document.title=PROGRAM.title||'NextStep';
  const url='data:image/svg+xml,'+encodeURIComponent(ICON);
  $('icon').href=url; $('touch-icon').href=url;
  validate();
  window.addEventListener('hashchange',route);
  route();
}

/* Kontrol: alle pas og trin skal have pace og puls */
function validate(){
  const chk=(st,where)=>{
    if(st.repeat){st.steps.forEach(x=>chk(x,where));return}
    if(!st.pace||!st.hr) console.error(`Mangler pace/puls: ${where}, trin "${KIND[st.kind]} ${st.dur}"`);
  };
  for(let w=1;w<=NW;w++)for(let d=0;d<7;d++){
    const s=session(w,d); if(!s) continue;
    const where=`uge ${w} ${DAYS[d]} – ${s.title}`;
    if(!s.pace||!s.hr) console.error(`Mangler pace/puls: ${where}`);
    s.steps.forEach(st=>chk(st,where));
  }
}
```

## 11. Tjekliste før levering
1. `index.html` er én fil; CSS og kerne er kopieret uændret fra `DesignSystem.md` og afsnit 9–10.
2. Siden åbner uden fejl i konsollen.
3. "I dag" virker for en dag med pas, en hviledag, før start og efter slut (brug `?dato=`).
4. Hver uge viser 7 dage; alle pas kan åbnes; "‹ Uge n" går tilbage.
5. Hvert pas (undtagen morgenløb) har en definitionsfil, og typen passer til dagen (speed → Speed; threshold/tempo → Threshold eller Tempo; long run → Long run; easy → Easy).
6. Gennemført-knappen gemmer og vises som "✓ Klaret" i ugen.
7. Alle pas og trin har både pace og puls – konsollen viser ingen "Mangler pace/puls"-fejl.
8. Lys og mørk tilstand ser rigtige ud; ingen vandret scroll ved 375px bredde.
