# Dokumentim Akademik: QR Event API

## 1. Titulli i projektit

**QR Event API - Sistem per gjenerimin dhe menaxhimin e kodeve QR per ngjarje, permbajtje dhe imazhe**

## 2. Abstrakti

Ky projekt paraqet zhvillimin e nje aplikacioni web dhe REST API per gjenerimin e kodeve QR ne format PNG. Sistemi mundeson krijimin e QR kodeve nga te dhena si URL, tekst i lire, informata per event, email, numer telefoni, SMS, WiFi, si dhe nga imazhe te ngarkuara ne hosting te jashtem. Pervec gjenerimit te imazhit PNG, aplikacioni ruan metadatat e QR kodeve ne databaze dhe ofron historik te gjenerimeve.

Projekti eshte zhvilluar me ASP.NET Core Web API, EF Core SQLite, JWT Authentication, Swagger/OpenAPI, QRCoder, SkiaSharp dhe HttpClient per integrimin me sherbimin e imazheve imgbb.

Qellimi kryesor i projektit eshte te demonstroje nje zgjidhje praktike, te strukturuar dhe te zgjerueshme per gjenerimin e QR kodeve per skenare reale, duke kombinuar backend API, frontend te thjeshte, ruajtje te te dhenave dhe integrim me sherbime te jashtme.

## 3. Hyrje

Kodet QR perdoren gjeresisht per ndarjen e shpejte te informatave ne menyre vizuale. Ato mund te permbajne linke, tekst, kontakte, kredenciale WiFi, informata per evente, imazhe dhe forma te tjera te dhenash. Ne kontekstin e eventeve, QR kodet jane te dobishme per regjistrim, promovim, qasje ne faqe informuese, ndarje te lokacionit, ndarje te imazheve ose automatizim te komunikimit.

Projekti **QR Event API** eshte krijuar si nje zgjidhje e plote qe ofron:

- Gjenerim te QR kodeve ne format PNG nga lloje te ndryshme permbajtjesh.
- Ngarkim te imazheve ne hosting te jashtem dhe gjenerim automatik te QR kodit nga URL-i i imazhit.
- Personalizim te ngjyrave, madhesise dhe logos.
- Preset-e per lloje te ndryshme QR kodeve: URL, tekst, event, email, telefon, SMS, WiFi dhe imazh.
- Ruajtje te historikut ne databaze.
- Autentikim me JWT per endpoint-et administrative.
- Dokumentim dhe testim me Swagger.
- Frontend te thjeshte per perdorim nga browser-i.

## 4. Qellimi i projektit

Qellimi i projektit eshte krijimi i nje REST API qe gjeneron QR kode nga te dhena te ndryshme dhe kthen rezultatin si imazh PNG. Sistemi synon te jete i lehte per perdorim, i qarte ne strukture dhe i pershtatshem per demonstrim akademik ose zhvillim te metejshem.

Objektivat kryesore jane:

1. Te ndertohet nje API per gjenerimin e QR kodeve nga permbajtje te ndryshme.
2. Te mundesohet ngarkimi i imazheve ne hosting te jashtem dhe gjenerimi i QR kodit nga URL-i i imazhit.
3. Te mundesohet personalizimi i QR kodit me ngjyra, madhesi dhe logo.
4. Te ofrohet nje frontend i thjeshte per perdoruesin me preset-e per cdo lloj QR.
5. Te ruhen metadatat e QR kodeve ne databaze.
6. Te sigurohen endpoint-et administrative me JWT.
7. Te dokumentohet API permes Swagger/OpenAPI.
8. Te testohet cilesia dhe shpejtesia e gjenerimit.

## 5. Teknologjite e perdorura

### 5.1 ASP.NET Core Web API

ASP.NET Core Web API perdoret per ndertimin e backend-it. Kjo teknologji mundeson krijimin e endpoint-eve REST, menaxhimin e kerkesave HTTP, validimin e te dhenave dhe kthimin e pergjigjeve ne formate te ndryshme si JSON dhe PNG. Perdoret gjithashtu per pranimin e skedareve permes `multipart/form-data` ne endpoint-in e ngarkimit te imazheve.

### 5.2 QRCoder

QRCoder perdoret per krijimin e struktures se QR kodit. Libraria gjeneron matricen e moduleve te QR kodit, e cila me pas renderohet si imazh PNG.

### 5.3 SkiaSharp

SkiaSharp perdoret per vizatimin grafik te QR kodit. Permes saj realizohet:

- Ngjyrosja e moduleve te QR kodit.
- Vendosja e sfondit.
- Renderimi i imazhit PNG.
- Vendosja e logos ne qender te QR kodit.

### 5.4 Entity Framework Core dhe SQLite

EF Core perdoret si ORM per komunikim me databazen, ndersa SQLite perdoret si databaze lokale. Sistemi ruan metadata si emri i eventit, lokacioni, permbajtja, ngjyrat, madhesia dhe data e krijimit.

### 5.5 JWT Authentication

JWT perdoret per sigurimin e endpoint-eve administrative. Per shembull, lista e historikut dhe fshirja e historikut kerkojne token autentikimi.

### 5.6 HttpClient dhe imgbb API

Per ngarkimin e imazheve projekti perdor `HttpClient` te ASP.NET Core per te komunikuar me imgbb, nje sherbim falas per hosting te imazheve. Kur perdoruesi ngarkon nje imazh, aplikacioni e konverton ne Base64 dhe e dergon ne imgbb permes nje kerkese HTTP. imgbb kthen nje URL publik te imazhit, i cili me pas perdoret si permbajtje per gjenerimin e QR kodit.

### 5.7 Swagger / OpenAPI

Swagger perdoret per dokumentimin dhe testimin e API-se. Ai mundeson shikimin e endpoint-eve, strukturave JSON dhe binar PNG, testimin e kerkesave dhe perdorimin e JWT token-it per endpoint-et e mbrojtura.

### 5.8 HTML, CSS dhe JavaScript

Frontend-i eshte ndertuar me HTML, CSS dhe JavaScript te thjeshte. Kjo e ben projektin me te lehte per kuptim, sepse nuk kerkon framework shtese si React ose Angular.

## 6. Arkitektura e sistemit

Projekti eshte organizuar sipas qasjes **feature-based structure**, ku skedaret grupohen sipas funksionalitetit dhe jo vetem sipas llojit teknik.

Struktura kryesore eshte:

```text
Features/
  Auth/
    AuthController.cs
    AuthModels.cs
    JwtTokenService.cs
  QrCodes/
    QrCodesController.cs
    QrCodeModels.cs
    QrCodeGeneratorService.cs
  Images/
    ImageUploadController.cs
    ImageUploadService.cs
Infrastructure/
  Data/
    AppDbContext.cs
wwwroot/
  index.html
  styles.css
  app.js
Program.cs
appsettings.json
```

Kjo strukture e ben projektin me te lehte per mirembajtje sepse secili funksionalitet ka kontrollorin, modelet dhe sherbimet e veta ne nje lokacion te qarte. Feature-t `Auth`, `QrCodes` dhe `Images` jane te pavarura nga njera-tjetra, por ndajne sherbimet e perbashketa si `IQrCodeGeneratorService`.

## 7. Pershkrimi i komponenteve kryesore

### 7.1 Program.cs

Skedari `Program.cs` eshte pika kryesore e konfigurimit te aplikacionit. Ne kete skedar konfigurohen:

- Controller-et.
- Swagger.
- EF Core me SQLite.
- JWT Authentication.
- Sherbimet e QR kodit dhe ngarkimit te imazheve.
- HttpClient per komunikim me imgbb API.
- Static files per frontend.
- Rrjedha e HTTP request pipeline.

Ky skedar lidh pjeset kryesore te aplikacionit dhe e ben API-ne te ekzekutueshme.

### 7.2 AuthController

`AuthController` ofron endpoint-in per login:

```http
POST /api/auth/login
```

Perdoruesi demo dergon `username` dhe `password`. Nese kredencialet jane te sakta, API kthen nje JWT token. Ky token perdoret per endpoint-et e mbrojtura, si historiku dhe fshirja e historikut.

### 7.3 JwtTokenService

`JwtTokenService` krijon token-in JWT. Token-i permban informata si:

- Subjekti i token-it.
- Emri i perdoruesit.
- ID unike e token-it.
- Koha e skadimit.
- Nenshkrimi me celes sekret.

### 7.4 ImageUploadService

`ImageUploadService` eshte sherbimi qe menaxhon ngarkimin e imazheve ne imgbb. Ky sherbim:

- Merr imazhin si `IFormFile` nga kontrollori.
- E konverton imazhin ne Base64.
- E dergon ne imgbb API permes `HttpClient` me `MultipartFormDataContent`.
- Deserializon JSON-in e kthyer dhe nxjerr URL-in publik te imazhit.
- Kthen URL-in tek kontrollori per gjenerimin e QR kodit.

API key-i i imgbb konfigurohet ne `appsettings.json` nen seksionin `ImgBb:ApiKey`.

### 7.5 ImageUploadController

`ImageUploadController` ofron endpoint-in per ngarkimin e imazheve dhe gjenerimin e QR kodit:

```http
POST /api/images/upload
```

Ky kontrollor:

- Merr imazhin si `multipart/form-data`.
- E kalon ne `ImageUploadService` per ngarkim ne imgbb.
- Merr URL-in publik te imazhit.
- Gjeneron QR kodin nga ai URL permes `IQrCodeGeneratorService`.
- Kthen PNG-ne e QR kodit me header-in `X-Image-Url` qe permban URL-in e imazhit.

### 7.6 QrCodesController

`QrCodesController` eshte kontrollori per gjenerimin dhe menaxhimin e QR kodeve. Ai ofron endpoint-et per gjenerim nga permbajtje tekstuale, lexim historiku, shkarkim dhe fshirje.

Funksionet kryesore jane:

- Pranon request-in per QR me permbajtje tekstuale.
- Gjeneron PNG permes `IQrCodeGeneratorService`.
- Ruan metadatat ne databaze.
- Kthen imazhin PNG.
- Liston historikun.
- Fshin historikun.

### 7.7 QrCodeGeneratorService

`QrCodeGeneratorService` permban logjiken e gjenerimit te QR kodit dhe perdoret nga te dy kontrolleret: `QrCodesController` dhe `ImageUploadController`. Ky sherbim:

- Merr permbajtjen qe duhet koduar — URL, tekst ose URL i imazhit.
- Zgjedh nivelin e error correction.
- Krijon matricen QR me QRCoder.
- E vizaton QR kodin me SkiaSharp.
- Vendos logon nese eshte dhene.
- E kthen rezultatin si byte array PNG.

### 7.8 AppDbContext

`AppDbContext` eshte klasa qe lidh aplikacionin me databazen SQLite. Ajo permban tabelen:

```csharp
public DbSet<QrCodeRecord> QrCodes => Set<QrCodeRecord>();
```

Kjo tabele ruan metadatat e QR kodeve te gjeneruara nga permbajtje tekstuale.

## 8. Funksionalitetet kryesore

### 8.1 Gjenerimi i QR kodit nga permbajtje tekstuale

Per gjenerimin e QR kodit nga URL, tekst ose permbajtje te tjera perdoret endpoint-i:

```http
POST /api/qrcodes
```

Shembull request:

```json
{
  "eventName": "Konference Teknologjie",
  "location": "Prishtine",
  "content": "https://example.com/events/konference",
  "foregroundColor": "#000000",
  "backgroundColor": "#FFFFFF",
  "size": 512,
  "logoBase64": null,
  "logoSizePercent": 18,
  "quietZoneModules": 4,
  "includeEventDetails": false,
  "errorCorrection": "H"
}
```

Pergjigja e API-se eshte nje imazh PNG. Header-i `X-QR-Code-Id` permban ID-ne e rekordit te ruajtur ne databaze.

### 8.2 Ngarkimi i imazhit dhe gjenerimi i QR kodit

Sistemi mundeson ngarkimin e nje imazhi direkt nga perdoruesi dhe gjenerimin automatik te QR kodit qe con tek ai imazh. Rrjedha eshte:

1. Perdoruesi dergon nje imazh ne endpoint-in:

```http
POST /api/images/upload
Content-Type: multipart/form-data
Body: file=<imazhi>
```

2. API-ja e konverton imazhin ne Base64 dhe e ngarkon ne imgbb.com.
3. imgbb kthen nje URL publik te imazhit te ngarkuar.
4. API-ja gjeneron nje QR kod PNG nga ai URL.
5. Pergjigja eshte PNG-ja e QR kodit.
6. Header-i `X-Image-Url` permban URL-in e imazhit te ngarkuar.

Shembull pergjigje:

```
HTTP/1.1 200 OK
Content-Type: image/png
X-Image-Url: https://i.ibb.co/abc123/foto.png

[PNG binary data]
```

Kur perdoruesi e skano QR kodin me telefon, hapet direkt imazhi i ngarkuar.

### 8.3 Personalizimi i QR kodit

Sistemi mundeson personalizimin e QR kodit me keto fusha:

- `foregroundColor`: ngjyra e moduleve te QR kodit.
- `backgroundColor`: ngjyra e sfondit.
- `size`: madhesia e imazhit.
- `logoBase64`: logo opsionale ne qender.
- `logoSizePercent`: madhesia e logos ne perqindje.
- `quietZoneModules`: margjina rreth QR kodit.
- `errorCorrection`: niveli i korrigjimit te gabimeve.

Default-i i projektit eshte QR klasik bardh e zi:

```json
{
  "foregroundColor": "#000000",
  "backgroundColor": "#FFFFFF"
}
```

### 8.4 Preset-et e QR kodeve

Frontend-i ofron preset-e per keto lloje QR:

- URL / Link
- Tekst i lire
- Event
- Email
- Telefon
- SMS
- WiFi
- Imazh → QR

Keto preset-e e ndihmojne perdoruesin te krijoje permbajtjen e duhur pa pasur nevoje te dije formatet teknike. Per shembull:

- Email krijohet si `mailto:...`
- Telefon krijohet si `tel:...`
- SMS krijohet si `SMSTO:...`
- WiFi krijohet me formatin `WIFI:T:...;S:...;P:...;;`
- Imazhi ngarkuar ne imgbb dhe QR kodi gjenerohet nga URL-i i kthyer

### 8.5 Historiku

Sistemi ruan historikun e QR kodeve ne databaze. Historiku mund te lexohet me:

```http
GET /api/qrcodes
```

Ky endpoint eshte i mbrojtur me JWT. Kjo do te thote se perdoruesi duhet te jete i autentikuar per ta pare historikun.

### 8.6 Fshirja e historikut

Per fshirjen e historikut perdoret endpoint-i:

```http
DELETE /api/qrcodes/history
```

Ky endpoint fshin te gjitha rekordet nga tabela `QrCodes` dhe kthen numrin e rekordeve te fshira.

Shembull pergjigje:

```json
{
  "deleted": 21
}
```

Edhe ky endpoint eshte i mbrojtur me JWT.

## 9. Frontend-i i aplikacionit

Frontend-i gjendet ne dosjen `wwwroot` dhe perbehet nga:

- `index.html`
- `styles.css`
- `app.js`

Faqja kryesore hapet ne:

```text
http://localhost:5201
```

Frontend-i ofron:

- Zgjedhje te llojit te QR kodit, perfshire opsionin per ngarkim imazhi.
- Fushe per emrin e eventit dhe vendndodhjen.
- Fushe per URL ose tekst.
- Input per ngarkim imazhi kur zgjidhet lloji "Imazh → QR".
- Zgjedhje te ngjyrave te QR kodit dhe sfondit.
- Zgjedhje te madhesise.
- Ngarkim te logos opsionale ne qender te QR kodit.
- Gjenerim dhe shfaqje te QR kodit ne preview.
- Shkarkim te PNG-se.
- Shfaqje dhe fshirje te historikut.

Frontend-i komunikon me API-ne permes `fetch`. Per opsionin e imazhit perdoret `FormData` dhe kerkohet `POST /api/images/upload`. Per endpoint-et e mbrojtura, ai ben login automatik me user-in demo dhe dergon token-in ne header-in `Authorization`.

## 10. Databaza

Databaza perdor SQLite dhe ruan tabelen `QrCodes`. Fushat kryesore jane:

| Fusha | Pershkrimi |
| --- | --- |
| Id | Identifikues unik i QR kodit |
| EventName | Emri i eventit |
| Location | Vendndodhja |
| Content | Permbajtja e koduar ne QR |
| ForegroundColor | Ngjyra e QR kodit |
| BackgroundColor | Ngjyra e sfondit |
| Size | Madhesia e imazhit |
| HasLogo | Tregon nese QR ka logo |
| CreatedAtUtc | Data e krijimit |

Databaza krijohet automatikisht kur aplikacioni startohet, permes `EnsureCreated()`.

## 11. Siguria dhe autentikimi

Projekti perdor JWT Bearer Authentication. Endpoint-i i login-it kthen nje token, i cili perdoret ne endpoint-et e mbrojtura.

Endpoint-et publike:

- `POST /api/auth/login`
- `POST /api/qrcodes`
- `POST /api/images/upload`

Endpoint-et e mbrojtura:

- `GET /api/qrcodes`
- `GET /api/qrcodes/{id}`
- `GET /api/qrcodes/{id}/png`
- `DELETE /api/qrcodes/history`

Endpoint-et e gjenerimit dhe ngarkimit te imazheve jane publike, sepse keto jane veprime krijuese qe nuk kerkojne identifikim. Menaxhimi i historikut eshte i mbrojtur sepse permban te dhena te ruajtura.

## 12. Swagger dhe dokumentimi i API-se

Swagger hapet ne:

```text
http://localhost:5201/swagger
```

Permes Swagger mund te:

- Shihen te gjitha endpoint-et, perfshire `POST /api/images/upload`.
- Testohen request-et me JSON ose skedare.
- Vendoset JWT token per endpoint-et e mbrojtura.
- Shihen modelet JSON.
- Testohet kthimi i PNG-se nga API.

Per endpoint-et e mbrojtura, ne Swagger duhet perdorur butoni `Authorize` dhe token-i vendoset ne kete format:

```text
Bearer <accessToken>
```

## 13. Testimi

Gjate zhvillimit jane kryer disa teste praktike:

1. Testim i build-it me `dotnet build`.
2. Testim i frontend-it me `http://localhost:5201`.
3. Testim i Swagger-it me `http://localhost:5201/swagger`.
4. Testim i gjenerimit te QR kodit me payload JSON per lloje te ndryshme.
5. Testim i ngarkimit te imazhit permes `POST /api/images/upload` dhe skanimi i QR kodit te gjeneruar.
6. Testim i ruajtjes se historikut ne SQLite.
7. Testim i fshirjes se historikut me `DELETE /api/qrcodes/history`.
8. Testim i QRCode Monkey si sherbim i jashtem krahasues.

Nga testimi i fshirjes se historikut u konfirmua se endpoint-i fshin rekordet me sukses. Shembull rezultati:

```text
Before=21
Deleted=21
After=0
```

Nga testimi i ngarkimit te imazhit u konfirmua se QR kodi i gjeneruar con direkt tek imazhi i ngarkuar ne imgbb kur skanonmohet me telefon.

## 14. Krahasimi me QRCode Monkey

Gjate projektit u testua edhe sherbimi QRCode Monkey. Ky sherbim ktheu PNG te vlefshem me kohe pergjigjeje rreth `379 ms`. Ne krahasim me te, projekti **QR Event API** ofron perparesi specifike:

- Gjenerimi i QR kodit kontrollohet plotesisht nga aplikacioni.
- Permbajtja e QR kodit mund te jete URL, tekst, informata per event, ose URL i imazhit te ngarkuar.
- Te dhenat e QR kodit ruhen ne databaze lokale.
- Mund te personalizohet dhe zgjerohet sipas nevojes.
- Integron hosting te imazheve permes imgbb per skenaret ku QR kodi duhet te con direkt tek nje imazh.

## 15. Perparesite e projektit

Perparesite kryesore jane:

- Strukture e qarte dhe e organizuar sipas feature-based architecture.
- Backend dhe frontend ne te njejtin projekt.
- Mbeshtetje per lloje te ndryshme QR: URL, tekst, event, email, telefon, SMS, WiFi dhe imazh.
- Ngarkim i imazheve ne hosting te jashtem dhe gjenerim i QR kodit nga URL-i i imazhit.
- Personalizim i QR kodit me ngjyra, madhesi dhe logo.
- Preset-e praktike ne frontend per cdo lloj QR.
- Ruajtje dhe fshirje e historikut ne databaze.
- JWT per mbrojtjen e endpoint-eve administrative.
- Swagger per testim dhe dokumentim.
- Integrim me HttpClient te ASP.NET Core per komunikim me API te jashtme.

## 16. Kufizimet aktuale

Projekti funksionon mire per qellime demonstrimi dhe perdorim lokal, por ka disa kufizime:

- Perdor user demo ne konfigurim, pa menaxhim te plote te perdoruesve.
- Nuk ka migrime te avancuara te databazes.
- Nuk ruan vet imazhin PNG te QR kodit ne databaze, por vetem metadata.
- Nuk ka validim te avancuar per te gjitha formatet si WiFi, email dhe telefon.
- Imazhet e ngarkuara varen nga disponueshmeria e sherbimit imgbb.

## 17. Mundesi per zhvillim te metejshem

Ne te ardhmen projekti mund te zgjerohet me:

- Regjistrim dhe menaxhim perdoruesish me role dhe permissions.
- Ruajtje e PNG-ve lokalisht ose ne cloud storage te dedikuar.
- Eksport ne PDF.
- Gjenerim batch per shume QR kode ne te njejten kohe.
- Statistika dhe analitike per QR kodet e krijuara.
- Dashboard administrativ.
- Migrime EF Core per menaxhim me te avancuar te skemes se databazes.
- Validim me te avancuar per preset-et e ndryshme.
- Deployment ne cloud.

## 18. Perfundim

Projekti **QR Event API** demonstron nje implementim praktik te nje REST API per gjenerimin e kodeve QR ne format PNG per skenare te ndryshme reale. Aplikacioni kombinon teknologji moderne si ASP.NET Core Web API, EF Core SQLite, JWT, Swagger, QRCoder, SkiaSharp dhe HttpClient per integrim me sherbime te jashtme. Sistemi mbeshtet gjenerim te QR kodeve nga lloje te ndryshme permbajtjesh tekstuale, si dhe ngarkim te imazheve ne hosting te jashtem me gjenerim automatik te QR kodit nga URL-i i imazhit.

Nga aspekti akademik, projekti tregon perdorimin e arkitektures se ndare sipas funksionaliteteve, implementimin e API-ve REST, sigurimin me JWT, punen me databaze, integrimin e nje frontend-i te thjeshte me backend-in dhe komunikimin me API te jashtme permes HttpClient. Per kete arsye, projekti eshte i pershtatshem si shembull praktik per zhvillim web, API design, menaxhim te te dhenave dhe integrim sherbimesh.
