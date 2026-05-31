# 🎬 Film Değerlendirme Uygulaması

**Film Değerlendirme Uygulaması**, C# Windows Forms (.NET Framework 4.7.2) ile geliştirilmiş, modern ve dinamik arayüze sahip kapsamlı bir film inceleme, oylama ve yönetim sistemidir. Güçlü bir SQLite veritabanı altyapısı üzerine kurulu olan uygulama, tamamen özelleştirilmiş **Dark Theme (Koyu Tema)** tasarımıyla modern dijital platformların (Netflix/IMDb) kullanıcı deneyimini masaüstüne taşır.

---

## 🌟 Öne Çıkan Özellikler

*   **Çift Rol Desteği (Role-Based Access Control):** Gelişmiş güvenlik ve yetkilendirme modeli. Admin ve normal kullanıcılar için dinamik olarak değişen arayüz.
*   **Koyu Tema ve Modern Arayüz:** `#1E1E1E` (Derin Koyu Gri), `#DC143C` (Kızıl Kırmızı), `#FFD700` (Puanlama Altını) renk paletiyle göz yormayan, üst segment Premium tasarım.
*   **Gelişmiş Giriş Ekranı (LoginForm):** Giriş yöntemini seçme paneli, Admin doğrulama ekranı, geri butonu ve altında şifre ipucu desteği.
*   **Merkezi Oturum Yönetimi:** Formların sağ üst köşesinde konumlandırılmış hızlı "Çıkış ✕" butonu ile güvenli çıkış ve giriş ekranına yönlendirme.
*   **Gerçek Zamanlı Filtreleme:** Arama çubuğu ile isme göre anında filtreleme ve film türüne (Genre) göre akıllı sınıflandırma.
*   **Akıllı Sıralama:** Filmleri "En Yüksek Puan" veya "En Yeni Eklenenler" kriterlerine göre tek tıkla listeleme.
*   **Dinamik Değerlendirme Sistemi:** Kullanıcı adı, 1-5 yıldızlı puanlama sistemi ve geniş yorum alanı ile filmlere inceleme ekleme.
*   **Otomatik Veritabanı ve Hazır Veriler (Database Seeding):** İlk kurulumda otomatik veritabanı oluşturma ve kullanıcının uygulamayı anında deneyimleyebilmesi için hazır gelen **5 popüler film ve bunlara ait hazır yorumlar**.

---

## 🔑 Kullanıcı Rolleri ve Yetkileri

Uygulama başlatıldığında modern bir giriş ekranı (`LoginForm`) açılır. Kullanıcılar buradan iki rolden biriyle devam edebilirler:

| Yetki / Özellik | 👤 Kullanıcı Modu (Şifresiz Giriş) | 🔐 Admin Modu (admin / admin123) |
| :--- | :---: | :---: |
| **Giriş Şekli** | Doğrudan tek tıkla giriş yapılabilir | Kullanıcı adı ve Şifre doğrulaması gerektirir |
| **Film Listeleme & Arama** | ✅ Aktif | ✅ Aktif |
| **Detay & Yorum İnceleme** | ✅ Aktif | ✅ Aktif |
| **Yorum Ekleme & Puanlama** | ✅ Aktif | ❌ Engelli (Yorum paneli gizlenir) |
| **Film Ekleme (Add Movie)** | ❌ Engelli (Ekleme butonu gizlenir) | ✅ Aktif |
| **Film Silme (Delete Movie)** | ❌ Engelli (Silme butonu gizlenir) | ✅ Aktif (Detay sayfasında görünür) |

> [!TIP]
> **Admin Giriş Bilgileri:** Giriş ekranında admin butonunun hemen altında ipucu olarak gösterilmektedir.
> *   **Kullanıcı Adı:** `admin`
> *   **Şifre:** `admin123`

---

## 📂 Proje Mimarisi ve Klasör Yapısı

Proje, katmanlı mimariye (MVC esintili) uygun şekilde organize edilmiştir:

```text
FilmReviewApp/
├── Configuration/            # Uygulama genel ayarları ve yapılandırmalar
├── Controls/
│   └── MovieCard.cs          # Ana sayfada filmleri listeleyen şık özel kart bileşeni
├── Data/
│   └── DatabaseHelper.cs     # SQLite CRUD işlemleri ve veritabanı yönetim sınıfı
├── Forms/
│   ├── AddMovieForm.cs       # Film ekleme/düzenleme modal arayüzü
│   ├── LoginForm.cs          # Kullanıcı türü seçimi ve Admin doğrulama arayüzü
│   └── MovieDetailForm.cs    # Detay görüntüleme, yorum yazma ve silme arayüzü
├── Models/
│   ├── Movie.cs              # Film veri modeli
│   └── Review.cs             # Yorum ve puanlama veri modeli
├── Utilities/                # Yardımcı araçlar ve UI renklendirme sınıfları
├── Form1.cs                  # Ana ekran formu (Film Galerisi ve filtreleme paneli)
├── Program.cs                # Uygulama başlangıç noktası (Giriş kontrolü)
└── FilmReviewApp.csproj      # .NET MSBuild proje dosyası
```

---

## 💾 Veritabanı ve Şema Yapısı

Uygulama, yerel bir **SQLite 3** veritabanı dosyası kullanır. Tablolar ilişkisel veri yapısına göre tasarlanmıştır ve film silindiğinde ilişkili tüm yorumlar otomatik olarak silinir (`ON DELETE CASCADE`).

### 1. Films (Film Bilgileri)
```sql
CREATE TABLE Films (
    FilmID INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Genre TEXT,
    Director TEXT,
    ReleaseYear INTEGER,
    Description TEXT,
    PosterPath TEXT
);
```

### 2. Reviews (Kullanıcı Değerlendirmeleri)
```sql
CREATE TABLE Reviews (
    ReviewID INTEGER PRIMARY KEY AUTOINCREMENT,
    FilmID INTEGER NOT NULL,
    UserName TEXT NOT NULL,
    Rating INTEGER NOT NULL,
    Comment TEXT,
    ReviewDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY(FilmID) REFERENCES Films(FilmID) ON DELETE CASCADE
);
```

---

## 🚀 Başlangıç ve Hazır Veriler (Data Seeding)

Uygulama başka bir cihazda veya yeni bir dizinde ilk kez çalıştırıldığında, veritabanı otomatik olarak oluşturulur ve **SeedDefaultMovies** mekanizması devreye girer. Bu mekanizma şu verileri yükler:

### 🎬 Hazır Yüklenen Filmler
1.  **Inception** (Bilim Kurgu - Yönetmen: Christopher Nolan - 2010)
2.  **Interstellar** (Bilim Kurgu - Yönetmen: Christopher Nolan - 2014)
3.  **The Matrix** (Aksiyon - Yönetmen: Wachowski Kardeşler - 1999)
4.  **Titanic** (Romantik - Yönetmen: James Cameron - 1997)
5.  **Avatar** (Bilim Kurgu - Yönetmen: James Cameron - 2009)

### 💬 Hazır Yüklenen Örnek Yorumlar
Her filme ait, farklı puanlara (1-5 arası) sahip hazır Türkçe kullanıcı yorumları eklenir. Böylece uygulama ilk açıldığı andan itibaren canlı ve zengin görünür, boş bir ekranla karşılaşılmaz.

---

## 🛠️ Kullanım Kılavuzu

### 1. Kurulum ve Çalıştırma
1.  Projeyi **Visual Studio** (tercihen 2019 veya üzeri) ile açın.
2.  NuGet Paket Yöneticisi üzerinden `System.Data.SQLite` paketinin yüklü olduğundan emin olun.
3.  Projeyi derleyin (Build) ve başlatın (F5).

### 2. Kullanıcı Girişi Yapma
1.  Karşınıza gelen ekrandan **"👤 Kullanıcı Olarak Devam Et"** butonuna basarak doğrudan ana paneli açabilirsiniz.
2.  Yorumları okuyabilir, arama çubuğundan film filtreleyebilir ve filmlere tıklayarak kendi yorumlarınızı yazabilirsiniz.
3.  Sağ üst köşedeki **"Çıkış ✕"** butonuna tıklayarak tekrar rol seçme ekranına dönebilirsiniz.

### 3. Yönetici (Admin) Girişi Yapma
1.  Ana ekrandan **"🔐 Admin Girişi"** butonuna basın.
2.  Açılan alanda kullanıcı adı olarak `admin`, şifre olarak `admin123` yazıp giriş yapın.
3.  Başarılı giriş sonrasında ana ekranda yeşil renkli **"+ Yeni Film Ekle"** butonu aktif olacaktır.
4.  Film detay formlarında ise **"Filmi Sil"** seçeneği aktif duruma gelir.

### 4. Yeni Film Ekleme (Yalnızca Admin)
1.  **"+ Yeni Film Ekle"** butonuna tıklayın.
2.  Açılan pencerede Film Adı, Tür, Yönetmen, Çıkış Yılı ve Açıklama bilgilerini girin.
3.  **"Afiş Seç"** butonuna tıklayarak bilgisayarınızdan görsel (JPG, PNG vb.) ekleyin.
4.  **"Filmi Kaydet"** butonuna tıklayın. Film anında ana ekranda kart olarak listelenecektir.

---

## 💎 Kod Kalitesi ve Tasarım Prensipleri

*   **Güvenlik (SQL Injection Koruması):** Veritabanı sorgularının tamamında dinamik metin birleştirme yerine **Parametreli Sorgular (Parameterized Queries)** kullanılarak tam koruma sağlanmıştır.
*   **Temiz ve Yorum Satırsız Kod (Clean Code):** Geliştirme sonrasında kod kalitesini artırmak ve gereksiz kalabalığı önlemek amacıyla, tüm geçici geliştirici yorum satırları kod tabanından temizlenmiştir.
*   **Performans & Hızlı Yanıt:** Arama ve sıralama işlemleri bellek (in-memory) üzerinde filtreleme ile anlık gerçekleşir. Detay sayfasındaki yorum listesi `DataGridView` ile performanslı bir şekilde veritabanından çekilir.
*   **Hata Yönetimi (Robust Exception Handling):** Tüm dosya ve SQLite bağlantı işlemlerinde gelişmiş `try-catch` blokları kullanılarak olası çalışma zamanı hataları kontrol altına alınmış ve kullanıcı dostu mesajlar verilmiştir.

---

## 💾 Yerel Veritabanı Dosya Yolu

SQLite veritabanı dosyanız (`films.db`) Windows üzerinde aşağıdaki dizinde güvenli bir şekilde saklanır:
```text
%APPDATA%\FilmReviewApp\films.db
```
*(Bu yol sisteminizde `C:\Users\<Kullanıcı_Adınız>\AppData\Roaming\FilmReviewApp\films.db` şeklindedir)*

---

**Uygulama Sürümü:** 1.0.0  
**Geliştirme Platformu:** .NET Framework 4.7.2 (C#)  
**Tasarım Dili:** Premium Dark Theme (C# WinForms Custom Render)
