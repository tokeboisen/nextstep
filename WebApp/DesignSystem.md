# NextStep designsystem

**Denne fil er det gældende designsystem for alle træningsprogrammer.** Claude bruger det altid, når et program laves eller opdateres. Sammen med `ApplicationStructure.md` er den hele kilden til appen – der findes ingen template. Designsystemet ejer det visuelle: tokens, typografi, layout, komponenter, CSS og ikon. Applikationsstrukturen ejer skærme, navigation, adfærd og kode.

## Principper
- **Én side, installérbar.** Appen er en single-page HTML-app: al CSS, JS og data ligger i én `index.html`. Ved siden af ligger kun det, der gør den til en installérbar app (PWA): manifest, service worker og PNG-ikoner (se `ApplicationStructure.md`, afsnit 1).
- **Løbebanen som motiv.** Mørk "lane"-hero med vandrette banestriber; pastypefarven som venstrekant og bjælke.
- **Tal i fokus.** Tid, km, pace og ugenumre i Barlow Condensed – læsbart i farten på en telefon.
- **Farve betyder pastype.** Farver bruges kun til pastyper og status – aldrig som pynt.
- **Lys og mørk.** Alle farver er tokens med lys og mørk værdi. Hårdkodede farver i komponenter er forbudt (undtagen hvid tekst på `--done`).

## Farvetokens

| Token | Lys | Mørk | Bruges til |
|---|---|---|---|
| `--bg` | #F3F5F4 | #10181F | Sidebaggrund |
| `--surface` | #FFFFFF | #18232C | Kort, knapper, bundmenu |
| `--ink` | #16232F | #E6ECEF | Tekst, aktiv tilstand |
| `--muted` | #5B6873 | #94A2AC | Sekundær tekst |
| `--line` | #D6DCDF | #2A3843 | Rammer, skillelinjer |
| `--soft` | #E9EDEE | #1F2C36 | Gentagelsesblokke, splits |
| `--lane` / `--laneText` | #16232F / #F3F5F4 | #0A1016 / #E6ECEF | Hero ("løbebane") |
| `--done` | #2F8A55 | #4CB47A | Gennemført |

### Pastype-farver

| `kind` | Token | Lys | Mørk | Label | Definitionsfilens type |
|---|---|---|---|---|---|
| `speed` | `--speed` | #C8372D | #E5584D | Speed | Speed |
| `thr` | `--thr` | #D99612 | #F0B63A | Threshold | Threshold |
| `tempo` | `--tempo` | #B5651D | #E08A4A | Tempo | Tempo |
| `long` | `--long` | #2E6E8E | #5AA0C4 | Long run | Long run |
| `easy` | `--easy` | #5C8F6B | #7DB38C | Rolig | Easy |
| `morning` | `--morning` | #8AA3B0 | #7F98A5 | Morgenløb | – |
| `test` / `race` | `--test` | #7A3FA0 | #B07BD6 | Testløb / Race | – |
| hvile | `--rest` | #A7B0B6 | #5E6B74 | Hvile | – |

Nye pastyper får et nyt token i både lys og mørk og en række i tabellen ovenfor, før de bruges.

## Typografi

| Rolle | Font | Størrelse / vægt |
|---|---|---|
| Brødtekst | Barlow | 16px, 400; linjehøjde 1.5 |
| Fremhævet tekst | Barlow | 600 |
| Hero-titel (h1) | Barlow Condensed | 700, `clamp(2.2rem,9vw,3.4rem)`, linjehøjde .98 |
| Sektionsoverskrift (h2) | Barlow Condensed | 700, 1.45rem |
| Tal (tid, km, pace, uge) | Barlow Condensed | 500/700 |
| Små labels | Barlow | 600, .72–.85rem |

Fonte hentes fra Google Fonts (eneste eksterne ressource) og caches af service workeren, så de også virker offline; systemfonte er fallback.

## Afstand, form og bevægelse
- Indholdsbredde: maks. 720px (`.wrap`), sidemargin 1rem
- Radius: 8px på kort og knapper, 6px på små knapper/felter, 50 % på ugeknapper; dagkort har 0 radius i venstre side (pastypekanten)
- Pastypekant: 5px venstrekant på dagkort og beskeder; 6px bjælke i hero
- Safe-area-insets respekteres i top og bund
- Bevægelse: kun en kort fade (.18s) ved skærmskift, og kun under `prefers-reduced-motion: no-preference`
- Fokus: `outline: 2px solid var(--thr)` med 2px offset på alle knapper

## Komponenter

| Komponent | Klasse | Anatomi |
|---|---|---|
| Hero | `.hero` | Banestriber, evt. tilbage-knap (`.back`), kicker, h1, chips (`.chips`), meta-tal (`.meta b` + `span`) – på pas altid med pace og puls, farvebjælke (`.bar` med `--c`) |
| Ugestribe | `.weekstrip` / `.wk` | Rund knap pr. uge; `.on` valgt (udfyldt), `.now` aktuel uge (rød prik), `.dl` deload (stiplet kant) |
| Ugenavigation | `.weeknav` / `.navbtn` / `.weektitle` | ‹ – datointerval + undertitel – › |
| Dagkort | `.daycard` | 3 kolonner: dag + dato (`.d`), titel (`h3`) + opsummering (`.sum`), tid (`.t`) + evt. "✓ Klaret"; `.today` ring; `.rest` hviledag uden klik |
| Tag | `.tag` | Lille ramme-label i titel, fx "Styrke A" |
| Kort | `.card` | Hvid flade med ramme – til Garmin-trin og tabeller |
| Trinliste | `.steps` / `.step` | Nummer (`.num`), type med farveprik (`.stype` + `.k-*`), varighed (`.sdur`), to `.stgt`-linjer – **Pace** og **Puls** (altid begge) – hvor Garmin-målet har en `.tag` "Garmin-mål", note (`.snote`) |
| Gentagelse | `.rep` / `.rephead` | Stiplet, blød blok "Gentag n× – note" med indlejrede trin |
| Regelliste | `.rules` | Punktliste |
| Splits | `.splits` | 4 felter: tid (`b`) + distance (`span`) |
| Tabel | `.tablewrap` / `table` | Vandret scroll på smal skærm; `td.n` holder tal på én linje |
| Besked | `.notice` | Kort med grå venstrekant: `h3` + tekst |
| Gennemført-knap | `.donebtn` | Fuld bredde; `.is` = grøn med hvid tekst |
| Bundmenu | `.tabs` / `.tab` | Fast i bunden, to faner; `.on` = mørk tekst + 3px topkant |

Trinprikkernes farver: `k-warm` og `k-cool` = `--long`, `k-run` = `--speed`, `k-rec` = `--easy`, `k-rest` = `--rest`.

## CSS (kanonisk)
Kopieres uændret ind i `<style>` i appen. Ændringer i designet laves her – aldrig kun i et program.

```css
:root{
  box-sizing:border-box;
  padding-top:env(safe-area-inset-top,0px);
  padding-bottom:env(safe-area-inset-bottom,0px);
  --bg:#F3F5F4; --surface:#FFFFFF; --ink:#16232F; --muted:#5B6873; --line:#D6DCDF; --soft:#E9EDEE;
  --speed:#C8372D; --thr:#D99612; --tempo:#B5651D; --long:#2E6E8E; --easy:#5C8F6B; --rest:#A7B0B6; --test:#7A3FA0; --morning:#8AA3B0;
  --lane:#16232F; --laneText:#F3F5F4; --done:#2F8A55;
}
@media (prefers-color-scheme: dark){
  :root:not([data-theme="light"]){
    --bg:#10181F; --surface:#18232C; --ink:#E6ECEF; --muted:#94A2AC; --line:#2A3843; --soft:#1F2C36;
    --speed:#E5584D; --thr:#F0B63A; --tempo:#E08A4A; --long:#5AA0C4; --easy:#7DB38C; --rest:#5E6B74; --test:#B07BD6; --morning:#7F98A5;
    --lane:#0A1016; --laneText:#E6ECEF; --done:#4CB47A;
  }
}
:root[data-theme="dark"]{
  --bg:#10181F; --surface:#18232C; --ink:#E6ECEF; --muted:#94A2AC; --line:#2A3843; --soft:#1F2C36;
  --speed:#E5584D; --thr:#F0B63A; --tempo:#E08A4A; --long:#5AA0C4; --easy:#7DB38C; --rest:#5E6B74; --test:#B07BD6; --morning:#7F98A5;
  --lane:#0A1016; --laneText:#E6ECEF; --done:#4CB47A;
}
html{scroll-padding-top:env(safe-area-inset-top,0px)}
*,*::before,*::after{box-sizing:inherit}
body{margin:0;background:var(--bg);color:var(--ink);font-family:"Barlow",system-ui,-apple-system,"Segoe UI",sans-serif;font-size:16px;line-height:1.5;-webkit-tap-highlight-color:transparent}
button{font:inherit;color:inherit}
.wrap{max-width:720px;margin:0 auto;padding:0 1rem}
main{padding-bottom:calc(5.5rem + env(safe-area-inset-bottom,0px))}
.cond{font-family:"Barlow Condensed","Arial Narrow",sans-serif}

/* hero */
.hero{background:var(--lane);color:var(--laneText);position:relative;overflow:hidden}
.hero::before{content:"";position:absolute;inset:0;background:repeating-linear-gradient(180deg,transparent 0 38px,rgba(255,255,255,.08) 38px 40px);pointer-events:none}
.hero .wrap{position:relative;padding-top:1.4rem;padding-bottom:1.3rem}
.hero .kicker{font-size:.95rem;opacity:.8;margin:0}
.hero h1{font-family:"Barlow Condensed","Arial Narrow",sans-serif;font-weight:700;font-size:clamp(2.2rem,9vw,3.4rem);line-height:.98;margin:.15rem 0 0}
.hero .meta{display:flex;flex-wrap:wrap;gap:1.4rem;margin-top:1rem}
.hero .meta b{display:block;font-family:"Barlow Condensed",sans-serif;font-size:1.6rem;line-height:1}
.hero .meta span{font-size:.82rem;opacity:.75}
.hero .bar{position:absolute;left:0;top:0;bottom:0;width:6px;background:var(--c,transparent)}
.back{background:none;border:0;padding:.2rem 0;margin:0 0 .4rem;color:var(--laneText);opacity:.85;cursor:pointer;font-size:.95rem}
.back:focus-visible,.navbtn:focus-visible,.daycard:focus-visible,.wk:focus-visible,.donebtn:focus-visible,.tab:focus-visible{outline:2px solid var(--thr);outline-offset:2px}

h2{font-family:"Barlow Condensed",sans-serif;font-weight:700;font-size:1.45rem;margin:1.7rem 0 .6rem}
p{margin:.4rem 0}
.muted{color:var(--muted)}

/* ugestribe */
.weekstrip{display:flex;gap:.4rem;overflow-x:auto;padding:.9rem 0 .2rem;scrollbar-width:none}
.weekstrip::-webkit-scrollbar{display:none}
.wk{flex:0 0 auto;min-width:2.6rem;height:2.6rem;border-radius:50%;border:1px solid var(--line);background:var(--surface);cursor:pointer;font-family:"Barlow Condensed",sans-serif;font-size:1.15rem;font-weight:500;position:relative}
.wk.on{background:var(--ink);color:var(--bg);border-color:var(--ink)}
.wk.now::after{content:"";position:absolute;bottom:3px;left:50%;width:5px;height:5px;margin-left:-2.5px;border-radius:50%;background:var(--speed)}
.wk.dl{border-style:dashed}
.weeknav{display:flex;align-items:center;justify-content:space-between;gap:.5rem;margin-top:.8rem}
.navbtn{border:1px solid var(--line);background:var(--surface);border-radius:6px;padding:.45rem .8rem;cursor:pointer;min-width:3rem}
.navbtn[disabled]{opacity:.35;cursor:default}
.weektitle{text-align:center}
.weektitle b{display:block;font-family:"Barlow Condensed",sans-serif;font-size:1.7rem;line-height:1.05}
.weektitle span{font-size:.9rem;color:var(--muted)}

/* dagkort */
.days{list-style:none;margin:1rem 0 0;padding:0}
.days li+li{margin-top:.5rem}
.daycard{display:grid;grid-template-columns:3.4rem 1fr auto;gap:.8rem;align-items:center;width:100%;text-align:left;background:var(--surface);border:1px solid var(--line);border-left:5px solid var(--c,var(--rest));border-radius:0 8px 8px 0;padding:.75rem .85rem;cursor:pointer}
.daycard.rest{cursor:default;background:transparent;border-color:var(--line) var(--line) var(--line) var(--rest)}
.daycard .d b{display:block;font-family:"Barlow Condensed",sans-serif;font-size:1.25rem;line-height:1}
.daycard .d span{font-size:.85rem;color:var(--muted)}
.daycard h3{margin:0;font-size:1.02rem;font-weight:600;line-height:1.25}
.daycard .sum{margin:.15rem 0 0;font-size:.88rem;color:var(--muted);line-height:1.35}
.daycard .t{font-family:"Barlow Condensed",sans-serif;font-size:1.2rem;text-align:right;white-space:nowrap}
.daycard .t small{display:block;font-family:"Barlow",sans-serif;font-size:.75rem;color:var(--done);font-weight:600}
.daycard.today{box-shadow:0 0 0 2px var(--ink)}
.tag{display:inline-block;font-size:.72rem;font-weight:600;border:1px solid var(--line);border-radius:3px;padding:0 .35rem;margin-left:.35rem;vertical-align:.1rem;color:var(--muted)}

/* pas */
.chips{display:flex;flex-wrap:wrap;gap:.4rem;margin:.6rem 0 0;padding:0;list-style:none}
.chips li{font-size:.85rem;border:1px solid rgba(255,255,255,.3);border-radius:4px;padding:.1rem .5rem}
.card{background:var(--surface);border:1px solid var(--line);border-radius:8px;padding:.9rem 1rem}
.steps{list-style:none;margin:0;padding:0}
.step{display:grid;grid-template-columns:1.7rem 1fr;gap:.6rem;padding:.65rem 0;border-bottom:1px solid var(--line)}
.step:last-child{border-bottom:0}
.step>.num{font-family:"Barlow Condensed",sans-serif;font-size:1.15rem;color:var(--muted);text-align:right;line-height:1.4}
.srow{display:flex;justify-content:space-between;gap:.6rem;align-items:baseline;flex-wrap:wrap}
.stype{font-weight:600}
.stype i{display:inline-block;width:.6rem;height:.6rem;border-radius:50%;margin-right:.4rem;background:var(--k);vertical-align:.05rem}
.sdur{font-family:"Barlow Condensed",sans-serif;font-size:1.25rem;line-height:1.1}
.stgt{font-size:.92rem;margin-top:.1rem}
.snote{font-size:.85rem;color:var(--muted);margin-top:.1rem}
.rep{border:1px dashed var(--line);border-radius:6px;padding:.3rem .7rem;margin-top:.1rem;background:var(--soft)}
.rep .step{grid-template-columns:1fr;padding:.5rem 0}
.rephead{display:flex;justify-content:space-between;font-weight:600;padding:.3rem 0 .15rem}
.k-warm{--k:var(--long)} .k-run{--k:var(--speed)} .k-rec{--k:var(--easy)} .k-rest{--k:var(--rest)} .k-cool{--k:var(--long)}
.rules{margin:0;padding-left:1.1rem}
.rules li{margin:.3rem 0}
.tablewrap{overflow-x:auto}
table{border-collapse:collapse;width:100%;font-size:.9rem}
th,td{text-align:left;padding:.45rem .5rem;border-bottom:1px solid var(--line);vertical-align:top}
th{font-weight:600;color:var(--muted)}
td.n{white-space:nowrap}
.donebtn{display:block;width:100%;margin-top:1.4rem;padding:.8rem;border-radius:8px;border:1px solid var(--line);background:var(--surface);font-weight:600;cursor:pointer}
.donebtn.is{background:var(--done);border-color:var(--done);color:#fff}
.notice{background:var(--surface);border:1px solid var(--line);border-left:5px solid var(--rest);border-radius:0 8px 8px 0;padding:.9rem 1rem;margin-top:1rem}
.notice h3{margin:0 0 .2rem;font-size:1.05rem}
.splits{display:grid;grid-template-columns:repeat(4,1fr);gap:.4rem;text-align:center;margin-top:.4rem}
.splits div{background:var(--soft);border-radius:6px;padding:.35rem 0}
.splits b{display:block;font-family:"Barlow Condensed",sans-serif;font-size:1.2rem}
.splits span{font-size:.75rem;color:var(--muted)}

/* bundmenu */
.tabs{position:fixed;left:0;right:0;bottom:0;background:var(--surface);border-top:1px solid var(--line);padding-bottom:env(safe-area-inset-bottom,0px);z-index:5}
.tabs .wrap{display:grid;grid-template-columns:1fr 1fr}
.tab{background:none;border:0;padding:.75rem 0 .8rem;cursor:pointer;font-weight:600;color:var(--muted);border-top:3px solid transparent}
.tab.on{color:var(--ink);border-top-color:var(--ink)}
@media (prefers-reduced-motion:no-preference){.view{animation:in .18s ease-out}@keyframes in{from{opacity:.4}to{opacity:1}}}
```

## Ikon (kanonisk)
En trappe på løbebane: tre lyse trin stiger mod højre, øverste trin har en rød top (`--speed`). SVG'en er kilden til alle ikoner: den indlejres i appen som favicon (se `ApplicationStructure.md`), og hjemmeskærmsikonerne er PNG-filer genereret fra den.

```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><rect width="512" height="512" fill="#16232F"/><g fill="#FFFFFF" fill-opacity=".08"><rect y="63" width="512" height="2"/><rect y="127" width="512" height="2"/><rect y="191" width="512" height="2"/><rect y="255" width="512" height="2"/><rect y="319" width="512" height="2"/><rect y="383" width="512" height="2"/><rect y="447" width="512" height="2"/></g><path fill="#F3F5F4" d="M88 312h112V200h112V120h112v304H88z"/><rect x="312" y="88" width="112" height="32" fill="#C8372D"/></svg>
```

### Maskable variant
Android beskærer hjemmeskærmsikoner til cirkel, dråbe m.m. og garanterer kun en central cirkel med radius 40 % af siden. Maskable-varianten er den kanoniske SVG, hvor trappen (ikke baggrund og banestriber) er skaleret til 75 % om midten (`translate(64 64) scale(.75)`), så alle hjørner ligger inden for den sikre zone.

```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><rect width="512" height="512" fill="#16232F"/><g fill="#FFFFFF" fill-opacity=".08"><rect y="63" width="512" height="2"/><rect y="127" width="512" height="2"/><rect y="191" width="512" height="2"/><rect y="255" width="512" height="2"/><rect y="319" width="512" height="2"/><rect y="383" width="512" height="2"/><rect y="447" width="512" height="2"/></g><g transform="translate(64 64) scale(.75)"><path fill="#F3F5F4" d="M88 312h112V200h112V120h112v304H88z"/><rect x="312" y="88" width="112" height="32" fill="#C8372D"/></g></svg>
```

### PNG-ikoner
Ligger i `WebApp/<Programnavn>/icons/` og er ens i alle programmer.

| Fil | Størrelse | Kilde | Bruges til |
|---|---|---|---|
| `icon-180.png` | 180 × 180 | Kanonisk SVG | `apple-touch-icon` (iPhone-hjemmeskærm) |
| `icon-192.png` | 192 × 192 | Kanonisk SVG | Manifest, `purpose: any` |
| `icon-512.png` | 512 × 512 | Kanonisk SVG | Manifest, `purpose: any` (installation, splash) |
| `icon-maskable-512.png` | 512 × 512 | Maskable variant | Manifest, `purpose: maskable` (Android) |

Generering: render SVG'en på et `<canvas>` i præcis ikonstørrelsen i en headless Chrome/Edge og gem `canvas.toDataURL('image/png')` som fil. Sådan en side, fx `render.html`:

```html
<pre id="o"></pre><script>
const SVG=`…SVG fra ovenfor…`, N=192;
const img=new Image();
img.onload=()=>{const c=document.createElement('canvas');c.width=c.height=N;c.getContext('2d').drawImage(img,0,0,N,N);o.textContent=c.toDataURL('image/png')};
img.src='data:image/svg+xml,'+encodeURIComponent(SVG);
</script>
```

køres med `chrome --headless --virtual-time-budget=3000 --dump-dom render.html`, og base64-delen af data-URL'en afkodes til `icon-192.png`. (`--screenshot --window-size` giver ikke præcise mål – vinduets ramme trækkes fra.) Ændres ikonet, ændres SVG'en her først, og alle PNG-filer genereres igen og kopieres til alle programmer.

### App-farver
| Felt | Værdi | Begrundelse |
|---|---|---|
| `theme-color` / manifest `theme_color` | #16232F | Lys `--lane` – statuslinje og titellinje matcher heroen |
| manifest `background_color` | #16232F | Splash-skærm ved opstart i samme farve som ikonets baggrund |

## Sprog og tekst
- Al tekst er på dansk; tal med dansk notation for tid (`4:30`, `90″`, `2′`)
- Knapper og labels er korte verber/navneord: "I dag", "Program", "Markér som gennemført", "✓ Gennemført"
- Opsummering af et pas følger mønstret `OP + hovedsæt + NL` (OP = opvarmning, NL = nedløb)
