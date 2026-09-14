# Coffee Shop

Kahve menüsü, kullanıcı kaydı/girişi ve sipariş yönetimi için Angular frontend + ASP.NET Core backend projesi.

```text
.
├── backend/CoffeeShop.Api   # ASP.NET Core 10 Web API (SQLite, JWT)
├── frontend                 # Angular 21 istemci
└── CoffeeShop.sln
```

## Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) 20+
- npm

## Backend

```bash
cd backend/CoffeeShop.Api
dotnet restore
dotnet run --launch-profile http
```

API varsayılan olarak `http://localhost:5000` adresinde ayağa kalkar.

JWT anahtarı geliştirme ortamında `appsettings.Development.json` içindedir. Üretimde bunu dosyaya yazmayın; `Jwt__Key` ortam değişkenini kullanın.

## Frontend

Başka bir terminalde:

```bash
cd frontend
npm install
npm start
```

Uygulama `http://localhost:4200` adresinde açılır. Geliştirme sunucusu `/api` isteklerini backend'e (`http://localhost:5000`) yönlendirir.

## API özeti

| Method | Endpoint | Açıklama |
| ------ | -------- | -------- |
| GET | `/api/coffees` | Kahve listesi (`?category=` ile filtre) |
| POST | `/api/auth/signup` | Kayıt |
| POST | `/api/auth/login` | Giriş (JWT) |
| POST | `/api/orders` | Sipariş oluştur (JWT) |
| GET | `/api/orders/my-orders` | Kullanıcı siparişleri (JWT) |

## Veritabanı

SQLite dosyası (`coffeeshop.db`) ilk çalıştırmada otomatik oluşur ve Git'e eklenmez. Şema değişiklikleri için:

```bash
cd backend/CoffeeShop.Api
dotnet ef database update
```
