
// kaynak kod :
Tesis tesis = new Tesis("Ana Kampüs Atölye Tesisi");
Atolye ahsapAtolyesi = new Atolye("Ahşap ");
Atolye metalAtolyesi = new Atolye("Metal ");

tesis.AtolyeEkle(ahsapAtolyesi);
tesis.AtolyeEkle(metalAtolyesi);

Kullanici kullanici1 = new Kullanici(101, "Ali Veli", true); 
Kullanici kullanici2 = new Kullanici(102, "Ayşe Kara", false);
tesis.KullaniciEkle(kullanici1);
tesis.KullaniciEkle(kullanici2);

Ekipman matkap = new Ekipman("Matkap", 1001);
Ekipman kaynakMakinesi = new Ekipman("Kaynak Makinesi", 2001);
ahsapAtolyesi.EkipmanEkle(matkap);
metalAtolyesi.EkipmanEkle(kaynakMakinesi);

YonetimSistemi sistem = new YonetimSistemi(tesis);

Console.WriteLine("\n--- Senaryo 1: Başarılı Ödünç Alma (Ahşap Atölyesi) ---");
Rezervasyon rezervasyon1 = sistem.OduncVer(101, "Ahşap ", 1001, 8); // Başarılı ödünç alma

Console.WriteLine("\n--- Senaryo 2: Eğitimi Olmayan Kullanıcı (Ahşap Atölyesi) ---");
sistem.OduncVer(102, "Ahşap ", 1001, 5); // Eğitimi olmayan kullanıcı denemesi 

Console.WriteLine("\n--- Senaryo 3: Bakımdaki Ekipman (Metal Atölyesi) ---");
kaynakMakinesi.BakimaAl();
sistem.OduncVer(101, "Metal ", 2001, 24); // Bakımdaki ekipmanı ödünç almaya çalışma
kaynakMakinesi.BakimdanCikar();

Console.WriteLine("\n--- Senaryo 4: Ödünç Alınan Ekipmanı İade Etme ---");
sistem.IadeAl(rezervasyon1); // Önceki rezervasyonu iade etme

Console.WriteLine("\n--- Senaryo 5: Başarılı Ödünç Alma (Metal Atölyesi) ---");
Rezervasyon rezervasyon2 = sistem.OduncVer(101, "Metal ", 2001, 10); // Başarılı ödünç alma
sistem.IadeAl(rezervasyon2);

Console.WriteLine("\n--- Senaryo 6: Eğitimli Kullanıcının Maksimum Süreyi Aşması ---");
sistem.OduncVer(101, "Ahşap ", 1001, 15); // 12 saati aşan bir süre denemesi



public enum EkipmanDurumu // Ekipmanın mevcut durumunu belirtir
{
    Musait,
    Oduncte,
    Bakimda
}

public class Kullanici // Kullanıcı bilgilerini tutar
{
    private int kullaniciID;
    private string adSoyad;
    private bool egitimTamamlandi;

    public int KullaniciID { get { return kullaniciID; } }
    public string AdSoyad { get { return adSoyad; } }
    public bool EgitimTamamlandi { get { return egitimTamamlandi; } }

    public Kullanici(int kullaniciID, string adSoyad, bool egitimTamamlandi) // Yeni kullanıcı oluşturur
    {
        this.kullaniciID = kullaniciID;
        this.adSoyad = adSoyad;
        this.egitimTamamlandi = egitimTamamlandi;
    }
}

public class Ekipman // Ekipman bilgilerini ve durumunu tutar
{
    private string isim;
    private int ekipmanID;
    private EkipmanDurumu durum;
    private Kullanici sonKullanici;
    public string EkipmanKodu { get; private set; }

    public string Isim { get { return isim; } }
    public int EkipmanID { get { return ekipmanID; } }
    public EkipmanDurumu Durum { get { return durum; } set { durum = value; } } 
    public Kullanici SonKullanici { get { return sonKullanici; } set { sonKullanici = value; } }
   
    public Ekipman(string isim, int ekipmanID) // Yeni ekipman oluşturur
    {
        this.isim = isim;
        this.ekipmanID = ekipmanID;
        this.durum = EkipmanDurumu.Musait;
        this.EkipmanKodu = $"EKP-{ekipmanID}";
    }
    public void BakimaAl() // Ekipmanı bakıma alır
    {
        durum = EkipmanDurumu.Bakimda;
        Console.WriteLine($"{isim} (ID: {ekipmanID}) bakıma alındı.");
    }
    public void BakimdanCikar() // Ekipmanı bakımdan çıkarır
    {
        durum = EkipmanDurumu.Musait;
        Console.WriteLine($"{isim} (ID: {ekipmanID}) bakımdan çıkarıldı ve tekrar kullanıma hazır.");
    }
}

public class Atolye // Atölye bilgilerini ve ekipman listesini tutar
{
    private string isim;
    private List<Ekipman> ekipmanlar;
    public string Isim { get { return isim; } }
    public List<Ekipman> Ekipmanlar { get { return ekipmanlar; } }

    public Atolye(string isim) // Yeni atölye oluşturur
    {
        this.isim = isim;
        this.ekipmanlar = new List<Ekipman>();
    }
   
    public void EkipmanEkle(Ekipman ekipman) // Yeni ekipman ekler
    {
        ekipmanlar.Add(ekipman);
        Console.WriteLine($"{ekipman.Isim} (ID: {ekipman.EkipmanID}) {isim} atölyesine eklendi.");
    }
    public Ekipman EkipmanBul(int ekipmanID) // Ekipman ID'sine göre ekipman bulur
    {
        return ekipmanlar.FirstOrDefault(e => e.EkipmanID == ekipmanID);
    }
    public bool EkipmanMusaitMi(int ekipmanID) // Ekipmanın müsait olup olmadığını kontrol eder
    {
        var ekipman = EkipmanBul(ekipmanID);
        return ekipman != null && ekipman.Durum == EkipmanDurumu.Musait;
    }
}

public class Tesis // Tesis bilgilerini, atölyeleri ve kullanıcıları tutar
{
    private string isim;
    private List<Atolye> atolyeListesi;
    private List<Kullanici> kullaniciListesi;
    public string Isim { get { return isim; } }
    public List<Atolye> AtolyeListesi { get { return atolyeListesi; } }
    public List<Kullanici> KullaniciListesi { get { return kullaniciListesi; } }
    public Tesis(string isim)
    {
        this.isim = isim;
        this.atolyeListesi = new List<Atolye>();
        this.kullaniciListesi = new List<Kullanici>();
    }
    public void AtolyeEkle(Atolye atolye) // Yeni atölye ekler
    {
        atolyeListesi.Add(atolye);
        Console.WriteLine($"{atolye.Isim} atölyesi tesise eklendi.");
    }
    public void KullaniciEkle(Kullanici kullanici) // Yeni kullanıcı ekler
    {
        kullaniciListesi.Add(kullanici);
        Console.WriteLine($"{kullanici.AdSoyad} (ID: {kullanici.KullaniciID}) kullanıcı olarak eklendi.");
    }
    public Atolye AtolyeBul(string atolyeIsmi) // Atölye ismine göre atölye bulur
    {
        return atolyeListesi.FirstOrDefault(a => a.Isim.Equals(atolyeIsmi, StringComparison.OrdinalIgnoreCase)); // Büyük/küçük harf duyarsız karşılaştırma
    }
    public Kullanici KullaniciBul(int kullaniciID) // Kullanıcı ID'sine göre kullanıcı bulur
    {
        return kullaniciListesi.FirstOrDefault(k => k.KullaniciID == kullaniciID); // LINQ kullanarak arama
    }
}
public class Rezervasyon // Rezervasyon bilgilerini tutar
{
    private static int rezervasyonSayaci = 1;
    private int rezervasyonID;
    private Kullanici kullanici;
    private Ekipman ekipman;
    private DateTime baslangicZamani;
    private DateTime bitisZamani;

    public int RezervasyonID { get { return rezervasyonID; } }
    public Kullanici Kullanici { get { return kullanici; } }
    public Ekipman Ekipman { get { return ekipman; } }
    public DateTime BaslangicZamani { get { return baslangicZamani; } }
    public DateTime BitisZamani { get { return bitisZamani; } }
    public Rezervasyon(Kullanici kullanici, Ekipman ekipman, int saat) // Yeni rezervasyon oluşturur
    {
        this.rezervasyonID = rezervasyonSayaci++;
        this.kullanici = kullanici;
        this.ekipman = ekipman;
        this.baslangicZamani = DateTime.Now;
        this.bitisZamani = baslangicZamani.AddHours(saat);
    }
    public void IadeEt(DateTime iadeTarihi) // Ekipman iade işlemini gerçekleştirir
    {
        if (iadeTarihi > bitisZamani)
        {
            Console.WriteLine($"Uyarı: {ekipman.Isim} (ID: {ekipman.EkipmanID}) iade süresi geçti. Gecikme olabilir.");
        }
        else
        {
            Console.WriteLine($"{ekipman.Isim} (ID: {ekipman.EkipmanID}) zamanında iade edildi.");
        }
        ekipman.Durum = EkipmanDurumu.Musait;
        ekipman.SonKullanici = kullanici;
        Console.WriteLine($"{ekipman.Isim} (ID: {ekipman.EkipmanID}) iade edildi ve tekrar kullanıma hazır.");
    }
}

public class YonetimSistemi // Tesis yönetim sistemini ve işlemlerini tutar
{
    private Tesis tesis;
    private List<Rezervasyon> rezervasyonlar;
    private const int MAKSIMUM_KULLANIM_SAATI_EGITIMSIZ = 4;
    private const int MAKSIMUM_KULLANIM_SAATI_EGITIMLI = 12;
    public YonetimSistemi(Tesis tesis)
    {
        this.tesis = tesis;
        this.rezervasyonlar = new List<Rezervasyon>();
    }
    public Rezervasyon OduncVer(int kullaniciID, string atolyeIsmi, int ekipmanID, int saat) // Ekipman ödünç alma işlemini gerçekleştirir
    {
        Kullanici kullanici = tesis.KullaniciBul(kullaniciID);
        if (kullanici == null)
        {
            Console.WriteLine($"Hata: Kullanıcı ID {kullaniciID} bulunamadı.");
            return null;
        }
        if (!kullanici.EgitimTamamlandi)
        {
            Console.WriteLine($"Hata: {kullanici.AdSoyad} (ID: {kullanici.KullaniciID}) gerekli eğitimi tamamlamamış.");
            return null;
        }


        Atolye atolye = tesis.AtolyeBul(atolyeIsmi); // Atölye ismine göre atölye bul
        if (atolye == null)
        {
            Console.WriteLine($"Hata: Atölye '{atolyeIsmi}' bulunamadı.");
            return null;
        }


        Ekipman ekipman = atolye.EkipmanBul(ekipmanID); // Atölyedeki ekipmanı bul
        if (ekipman == null)
        {
            Console.WriteLine($"Hata: Ekipman ID {ekipmanID} atölyede bulunamadı.");
            return null;
        }
        if (ekipman.Durum == EkipmanDurumu.Bakimda)
        {
            Console.WriteLine($"Hata: {ekipman.Isim} (ID: {ekipman.EkipmanID}) şu anda bakımdadır.");
            return null;
        }
        if (ekipman.Durum == EkipmanDurumu.Oduncte)
        {
            Console.WriteLine($"Hata: {ekipman.Isim} (ID: {ekipman.EkipmanID}) şu anda ödünçtedir.");
            return null;
        }
        int maxSaat = kullanici.EgitimTamamlandi ? MAKSIMUM_KULLANIM_SAATI_EGITIMLI : MAKSIMUM_KULLANIM_SAATI_EGITIMSIZ; // Maksimum kullanım süresini belirle
        if (saat > maxSaat)
        {
            Console.WriteLine($"Hata: {kullanici.AdSoyad} (ID: {kullanici.KullaniciID}) için maksimum kullanım süresi {maxSaat} saattir.");
            return null;
        }


        Rezervasyon rezervasyon = new Rezervasyon(kullanici, ekipman, saat);
        rezervasyonlar.Add(rezervasyon);
        ekipman.Durum = EkipmanDurumu.Oduncte;

        Console.WriteLine("--- Ödünç Başarılı ---");
        Console.WriteLine($"{ekipman.Isim} (ID: {ekipman.EkipmanID}) başarıyla {kullanici.AdSoyad} (ID: {kullanici.KullaniciID})'e ödünç verildi. Rezervasyon ID: {rezervasyon.RezervasyonID}");
        Console.WriteLine($"Ödünç Alma Zamanı: {rezervasyon.BaslangicZamani.ToString("g")}, İade Zamanı: {rezervasyon.BitisZamani.ToString("g")} (Maksimum {maxSaat} saat)");
        return rezervasyon;
    }
    public void IadeAl(Rezervasyon rezervasyon) // Ekipman iadesini işler
    {
        if (rezervasyon == null)
        {
            Console.WriteLine("Hata: Geçersiz rezervasyon.");
            return;
        }
        DateTime iadeTarihi = DateTime.Now;
        rezervasyon.IadeEt(DateTime.Now);
        rezervasyonlar.Remove(rezervasyon);

        Ekipman iadeEdilenEkipman = rezervasyon.Ekipman;
        Console.WriteLine("--- İade Başarılı ---");
        Console.WriteLine($"Ekipman: {iadeEdilenEkipman.EkipmanID}, Kodu: {iadeEdilenEkipman.EkipmanKodu}");
        Console.WriteLine($"Ödünç: {rezervasyon.BaslangicZamani.ToString("g")}, İade: {iadeTarihi.ToString("g")}");

        if (iadeEdilenEkipman.SonKullanici != null)
        {
            Console.WriteLine($"Önceki Kullanıcı: {iadeEdilenEkipman.SonKullanici.AdSoyad}");
        }
    }
}


