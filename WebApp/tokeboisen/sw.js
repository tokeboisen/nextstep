/* NextStep service worker – ens i alle programmer */
const SCOPE=new URL(self.registration.scope).pathname;
const CACHE='nextstep:'+SCOPE;
const APP=['./','manifest.webmanifest','icons/icon-180.png','icons/icon-192.png','icons/icon-512.png','icons/icon-maskable-512.png'];
const FONTS='https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@500;700&family=Barlow:wght@400;500;600&display=swap';
const FONT_HOSTS=['https://fonts.googleapis.com','https://fonts.gstatic.com'];

self.addEventListener('install',e=>e.waitUntil((async()=>{
  const c=await caches.open(CACHE);
  await c.addAll(APP.map(u=>new Request(u,{cache:'reload'})));
  try{                                        // fonte er valgfrie – fejl stopper ikke installationen
    const res=await fetch(FONTS);
    const css=await res.clone().text();
    await c.put(FONTS,res);
    await c.addAll([...css.matchAll(/url\((https:\/\/fonts\.gstatic\.com\/[^)]+)\)/g)].map(m=>m[1]));
  }catch(err){}
  await self.skipWaiting();
})()));

self.addEventListener('activate',e=>e.waitUntil(self.clients.claim()));

self.addEventListener('fetch',e=>{
  const req=e.request, url=new URL(req.url);
  if(req.method!=='GET') return;
  if(FONT_HOSTS.includes(url.origin)){
    e.respondWith(caches.match(req,{ignoreVary:true}).then(hit=>hit||fetch(req).then(res=>{
      const copy=res.clone(); caches.open(CACHE).then(c=>c.put(req,copy)); return res;
    })));
    return;
  }
  if(url.origin!==location.origin||!url.pathname.startsWith(SCOPE)) return;
  const key=req.mode==='navigate'?new Request('./'):req;
  e.respondWith((async()=>{
    const c=await caches.open(CACHE);
    const hit=await c.match(key,{ignoreSearch:true});
    const net=fetch(key,{cache:'no-cache'}).then(res=>{if(res.ok)c.put(key,res.clone());return res});
    if(hit){e.waitUntil(net.catch(()=>{}));return hit}
    return net;
  })());
});
