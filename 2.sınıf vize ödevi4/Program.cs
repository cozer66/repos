

Console.WriteLine("=== Film Kütüphanesi Senaryo Başlangıcı ===");
FilmYonetimi liste = new FilmYonetimi();

try { liste.FilmEkle(new aksiyon(13, 85, "Hızlı ve Öfkeli")); }
catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"\nHata: {ex.Message}"); }
// hızlı ve öfkeli 13 yaş sınırı ile eklendi
try { liste.FilmEkle(new komedi(7, 110, "Çılgın Dersane", 4)); }
catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"\nHata: {ex.Message}"); }
// çılgın dersane 7 yaş sınırı ile eklenemedi
try { liste.FilmEkle(new dram(18, 95, "The Shawshank Redemption")); } 
catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"\nHata: {ex.Message}"); }
// shawshank redemption 18 yaş sınırı ile eklenemedi
try { liste.FilmEkle(new belgesel(20, 75, "Planet Earth")); } 
catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"\nHata: {ex.Message}"); }
// planet earth 20 yaş sınırı ile eklendi


Console.WriteLine("\n--- KULLANICI EYLEMİ: Listeyi İlk Haliyle İnceleme ---");
liste.ListeyiGoster();
Console.WriteLine("\n--- KULLANICI EYLEMİ: Listeyi İsme Göre Sıralama ---");
liste.ListeyiSirala();
liste.ListeyiGoster();
Console.WriteLine("\n--- KULLANICI EYLEMİ: Listeyi Karıştırma ---");
liste.Karistir();
liste.ListeyiGoster();
Console.WriteLine("\n--- KULLANICI EYLEMİ: Toplam Maliyeti Hesaplama ---");
liste.ToplamMaliyetHesapla();
string filmAdi = "Hızlı ve Öfkeli";
Console.WriteLine($"\n--- KULLANICI EYLEMİ: '{filmAdi}' Filmini Listeden Çıkarma ---");
liste.FilmCikar(filmAdi);
liste.ListeyiGoster();
Console.WriteLine("\n=== Film Kütüphanesi Senaryo Sonu ===");




public interface IFilmIslem // Film işlemleri için arayüz
{
    void FilmEkle(film film);
    void FilmCikar(string ad);
    void ListeyiSirala();
    void Karistir();
    void ListeyiGoster();
    void ToplamMaliyetHesapla();
    void kurallarıdogrula();
}

public abstract class film // Film temel sınıfı
{
    public int yassınırı {  get; private set; }
    public int süre {  get; set; }
    public string ad {  get; set; }
    public abstract double Fiyat { get; }
    public film(int yassınırı, int süre, string ad)
    { // Film oluşturucu
        if ( yassınırı < 13) // Yaş sınırı kontrolü
        {
            throw new ArgumentOutOfRangeException($"'{ad}' filmi kural ihlali! Yaş sınırı 13 yada 13'den büyük olmalıdır.");
        }
        if (süre < 0) // Süre kontrolü
        {
            throw new ArgumentOutOfRangeException("Süre 0 ile 300 dakika arasında olmalıdır.");
        }

        this.yassınırı = yassınırı;
        this.süre = süre;
        this.ad = ad;

        kurallarıdogrula(); // Tür bazlı kuralları doğrula
    }
    public abstract void kurallarıdogrula(); // Tür bazlı kuralları doğrulama metodu
    public virtual string Goster() // Film bilgilerini gösterme metodu
    {
        return $"Ad: {ad}, Süre: {süre} dk, Yaş Sınırı: {yassınırı}, Tür: {this.GetType().Name}, Fiyat: {Fiyat} dolar";
    }
}
public class aksiyon : film // Aksiyon filmi sınıfı
{
    public override double Fiyat => 2.0; // Aksiyon filmlerinin fiyatı
    public aksiyon(int yassınırı, int süre, string ad ) : base(yassınırı, süre, ad)
    { // Aksiyon filmi oluşturucu
    }
    public override void kurallarıdogrula() // Aksiyon filmi kuralları doğrulama
    {
        if ( süre > 90)
        {
            throw new ArgumentOutOfRangeException($"Aksiyon filmlerinin süresi 90 dakikadan fazla olamaz. (Mevcut: {süre} dk)");
        }
    }
    public override string Goster() // Aksiyon filmi bilgilerini gösterme
    {
        return base.Goster();
    }
}
public class komedi : film // Komedi filmi sınıfı
{
    public override double Fiyat => 1.2;
    public int mizahseviyesi { get; private set; } // 1 ile 5 arasında mizah yoğunluğu
    public komedi(int yassınırı, int süre, string ad, int mizahyogunlugu) : base(yassınırı, süre, ad)
    {// Komedi filmi oluşturucu
        if (mizahyogunlugu < 1 || mizahyogunlugu > 5)
        {
            throw new ArgumentOutOfRangeException("Mizah yoğunluğu 1 ile 5 arasında olmalıdır.");
        }
        this.mizahseviyesi = mizahyogunlugu;
    }
    public override void kurallarıdogrula() // komedi filmi kuralları doğrulama
    {
        if (süre > 120)
        {
            throw new ArgumentOutOfRangeException($"Komedi filmlerinin süresi 120 dakikadan fazla olamaz. (Mevcut: {süre} dk)");
        }
    }
    public override string Goster()
    {
        return base.Goster();
    }
}
public class dram : film // Dram filmi sınıfı
{
    public override double Fiyat => 1.0;
    public dram(int yassınırı, int süre, string ad) : base(yassınırı, süre, ad)
    {
    }
    public override void kurallarıdogrula() // Dram filmi kuralları doğrulama
    {
        if (süre > 90)
        {
            throw new ArgumentOutOfRangeException($"Dram filmlerinin süresi 90 dakikadan fazla olamaz. (Mevcut: {süre} dk)");
        }
    }
    public override string Goster()
    {
        return base.Goster();
    }
}
public class belgesel : film // Belgesel filmi sınıfı
{
    public override double Fiyat => 0.5;
    public int belgeselsüresi { get; set; } // Belgesel süresi
    public belgesel(int yassınırı, int süre, string ad) : base(yassınırı, süre, ad)
    {
    }
    public override void kurallarıdogrula()
    {
        if (süre < 60 || süre > 180) // Belgesel süresi kontrolü
        {
            throw new ArgumentOutOfRangeException($"Belgesel filmlerinin süresi 45 ile 180 dakika arasında olamlıdır. (Mevcut: {süre} dk)");
        }
    }
    public override string Goster()
    {
        return base.Goster();
    }
}
public class FilmYonetimi : IFilmIslem // Film yönetimi sınıfı
{
    private List<film> filmler = new List<film>(); // Film listesi
    public void FilmEkle(film film) // Film ekleme metodu
    {
        filmler.Add(film);
        Console.WriteLine($"\n{film.ad} filmi listeye eklendi.");
    }
    public void FilmCikar(string ad) // Film çıkarma metodu
    {
        var film = filmler.FirstOrDefault(f => f.ad == ad);
        if (film != null)
        {
            filmler.Remove(film);
            Console.WriteLine($"\n{ad} adlı film listeden çıkarıldı.");
        }
        else
        {
            Console.WriteLine($"\n{ad} adlı film bulunamadı.");
        }
    }
    public void ListeyiSirala() // Film listesini isme göre sıralama metodu
    {
        filmler = filmler.OrderBy(f => f.ad).ToList();
        
    }
    public void Karistir() // Film listesini karıştırma metodu
    {
        var rnd = new Random();
        filmler = filmler.OrderBy(f => rnd.Next()).ToList();
        Console.WriteLine("\nListe karıştırıldı.");
    }
    public void ListeyiGoster() // Film listesini gösterme metodu
    {
        foreach (var film in filmler)
        {
            Console.WriteLine(film.Goster());
        }
    }
    public void ToplamMaliyetHesapla() // Toplam maliyeti hesaplama metodu
    {
        double toplamMaliyet = filmler.Sum(f => f.Fiyat);
        Console.WriteLine($"Toplam Maliyet: {toplamMaliyet} dolar");
    }
    public void kurallarıdogrula() // Kuralları doğrulama metodu
    {
    }
}
