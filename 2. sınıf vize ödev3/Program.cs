
using System.Text;

money kullanıcıbütçesi = new money(9000, money.parabirimi.TL);
Console.WriteLine($"Kullanıcı Bütçesi: {kullanıcıbütçesi.Tutar} {kullanıcıbütçesi.ParaBirimi}");
müşteri müşteri1 = new müşteri("Ayşe","Dolu",kullanıcıbütçesi);

try
{
    // laptop ürünü alımı
    money ürün1 = new money(1500, money.parabirimi.TL);
    ürün yeniÜrün1 = new ürün(2, "Laptop", ürün1, 8, 10);
    müşteri1.ürünekle(yeniÜrün1);

    // mouse ürünü alımı
    money ürün2 = new money(300, money.parabirimi.TL);
    ürün yeniÜrün2 = new ürün(1, "Mouse", ürün2, 8, 5);
    müşteri1.ürünekle(yeniÜrün2);

    // telefon ürünü alımı
    money ürün3 = new money(5000, money.parabirimi.TL);
    ürün yeniÜrün3 = new ürün(1, "Telefon", ürün3, 8, 20);
    müşteri1.ürünekle(yeniÜrün3);

    Console.WriteLine("\n--- Sepet İçeriği ve Hesaplamalar ---");
    Console.WriteLine(müşteri1.fatura());

    Console.WriteLine("\n--- Bütçe Yetersizliği Durumu ---");
    money ürün4 = new money(9000, money.parabirimi.TL);
    ürün yeniÜrün4 = new ürün(1, "Televizyon", ürün4, 8, 30);
    müşteri1.ürünekle(yeniÜrün4);// Bu satır bütçe yetersizliği nedeniyle hata fırlatacak
    Console.WriteLine(müşteri1.fatura());
}
catch (InvalidOperationException ex)// Bütçe yetersizliği durumu için özel hata yakalama
{
    Console.WriteLine($"İşlem Başarısız: {ex.Message}");
    Console.WriteLine($"Mevcut Fatura Durumu:\n {müşteri1.fatura()}");
}
catch (Exception ex)// Diğer hatalar için genel hata yakalama
{
    Console.WriteLine($"Hata: {ex.Message}");
}


public class money // Para Birimi Sınıfı
{
    private int tutar;
    private parabirimi paraBirimi;
    public int Tutar => tutar;
    public parabirimi ParaBirimi => paraBirimi;
    private static readonly Dictionary<parabirimi, double> DovizKurlari = new Dictionary<parabirimi, double>
    { // Örnek Döviz Kurları
        { parabirimi.TL, 1.0 },
        { parabirimi.USD, 32.0 },
        { parabirimi.EUR, 35.0 }  
    };

    public enum parabirimi // Para Birimi Türleri
    {
        TL = 1,
        USD = 2,
        EUR = 3
    }
    public money(int tutar, parabirimi paraBirimi) // Constructor
    {
        if (tutar < 0) // Tutar negatif olamaz hatası
        {
            throw new ArgumentOutOfRangeException("Tutar negatif olamaz.");
        }
        if(!Enum.IsDefined(typeof(parabirimi), paraBirimi)) // Geçersiz para birimi hatası
        {
            throw new ArgumentException("Geçersiz para birimi.");
        }
        this.tutar = tutar;
        this.paraBirimi = paraBirimi;
    }
    public money convertto(parabirimi hedefParaBirimi, double dövizKuru) // Para Birimi Dönüşümü
    {
        if (paraBirimi == hedefParaBirimi) { return new money(tutar, paraBirimi); } // Aynı para birimi ise dönüşüm yapma
        if (!DovizKurlari.ContainsKey(paraBirimi) || !DovizKurlari.ContainsKey(hedefParaBirimi))
        {
            throw new ArgumentException("Desteklenmeyen para birimi."); // Desteklenmeyen para birimi hatası
        }
        double tlTutar = tutar * DovizKurlari[paraBirimi];
        double hedefTutar = tlTutar / DovizKurlari[hedefParaBirimi];
        return new money((int)Math.Round(hedefTutar), hedefParaBirimi);
    }
    public static money operator +(money m1, money m2)
    {// Toplama Operatörü Aşırı Yükleme: Karşılaştırmayı ilk operandın biriminde yapar.
        // İkinci tutarı ilk tutarın birimine çevir
        money m2BütçeBirimi = m2.convertto(m1.ParaBirimi, 1.0);
        return new money(m1.Tutar + m2BütçeBirimi.Tutar, m1.ParaBirimi);
    } 
    public static money operator -(money m1, money m2)
    {// Çıkarma Operatörü Aşırı Yükleme: Karşılaştırmayı ilk operandın biriminde yapar.
        // İkinci tutarı ilk tutarın birimine çevir
        money m2BütçeBirimi = m2.convertto(m1.ParaBirimi,1.0);
        return new money(m1.Tutar - m2BütçeBirimi.Tutar, m1.ParaBirimi);
    }
    public static bool operator >(money m1, money m2)
    {// Büyüktür Operatörü Aşırı Yükleme: Karşılaştırmayı ilk operandın biriminde yapar.
        money m2BütçeBirimi = m2.convertto(m1.ParaBirimi, 1.0);
        return m1.Tutar > m2BütçeBirimi.Tutar;
    }

    
    public static bool operator <(money m1, money m2)
    {// Küçüktür Operatörü Aşırı Yükleme: Karşılaştırmayı ilk operandın biriminde yapar.
        money m2BütçeBirimi = m2.convertto(m1.ParaBirimi, 1.0);
        return m1.Tutar < m2BütçeBirimi.Tutar;
    }
    public override string ToString()
    {
        return $"{tutar} {paraBirimi}";
    }


}
public class müşteri // Müşteri Sınıfı
{
     public string ad;
     public string soyad;
     private readonly List<ürün> ürünler = new List<ürün>();
    private readonly money bütçe;
        public müşteri(string ad, string soyad, money bütçe) // Constructor
    {
            this.ad = ad;
            this.soyad = soyad;
            this.bütçe = bütçe ?? throw new ArgumentNullException(nameof(bütçe));
        }
    public void ürünekle(ürün yeniÜrün) // Ürün Ekleme Metodu
    { 
        int mevcutHarcanan = toplamHarcanan().Tutar;
        money yeniÜrünToplamFiyat = yeniÜrün.toplamFiyat();
        money yeniÜrünTLFiyat = yeniÜrünToplamFiyat.convertto(bütçe.ParaBirimi, 1.0);
        
        int yeniToplamHarcanan = mevcutHarcanan + yeniÜrünTLFiyat.Tutar;

        if (yeniToplamHarcanan > bütçe.Tutar)
        {
            throw new InvalidOperationException($"Bütçe yetersiz. Yeni ürünün maliyeti ({yeniÜrünTLFiyat.Tutar} {bütçe.ParaBirimi}) eklendiğinde toplam harcama bütçeyi aşmaktadır.");
        }
        ürünler.Add(yeniÜrün);
    }
    
    public money toplamHarcanan()
    {
        int toplam = 0;
        foreach (var ürün in ürünler)
        {
            money ürünToplamFiyat = ürün.toplamFiyat();
            money ürünBütçeParaBirimiFiyat = ürünToplamFiyat.convertto(bütçe.ParaBirimi, 1.0);
            
            toplam += ürün.toplamFiyat().Tutar;
        }
        return new money(toplam, bütçe.ParaBirimi);
    }
   
    public string fatura()
    {
        StringBuilder faturaMetni = new StringBuilder();
        faturaMetni.AppendLine($"Müşteri: {ad} {soyad}");
        faturaMetni.AppendLine("Ürünler:");
        foreach (var ürün in ürünler)
        { 
            money ürünBütçeParaBirimiFiyat = ürün.toplamFiyat().convertto(bütçe.ParaBirimi, 1.0);
            faturaMetni.AppendLine($"{ürün.ürünAdı} - Adet: {ürün.adet}, Birim Fiyat: {ürün.birimFiyat.Tutar} {ürün.birimFiyat.ParaBirimi}, KDV: {ürün.kdv}%, İndirim: {ürün.indirimOranı}% - Toplam: {ürün.toplamFiyat().Tutar} {ürün.birimFiyat.ParaBirimi}");
        }
        faturaMetni.AppendLine($"Toplam Harcanan: {toplamHarcanan().Tutar} {bütçe.ParaBirimi}");
        faturaMetni.AppendLine($"Kalan Bütçe: {bütçe.Tutar - toplamHarcanan().Tutar} {bütçe.ParaBirimi}");
        return faturaMetni.ToString();
    }
    

}

public class ürün
{
    public string ürünAdı;
    public money birimFiyat;
    public int adet;
    public int kdv;
    public int indirimOranı;
    public ürün(sbyte adet, string ürünAdı, money birimFiyat, int kdv, int indirimOranı)
    {
        if (adet <= 0)
        {
            throw new ArgumentOutOfRangeException("Adet pozitif olmalıdır.");
        }
        if (kdv < 0 || kdv > 100)
        {
            throw new ArgumentOutOfRangeException("KDV oranı 0 ile 100 arasında olmalıdır.");
        }
        if (indirimOranı < 0 || indirimOranı > 100)
        {
            throw new ArgumentOutOfRangeException("İndirim oranı 0 ile 100 arasında olmalıdır.");
        }
        this.adet = adet;
        this.ürünAdı = ürünAdı;
        this.birimFiyat = birimFiyat;
        this.kdv = kdv;
        this.indirimOranı = indirimOranı;
    }
    public money toplamFiyat()
    {
        int toplam = birimFiyat.Tutar * adet;
        int indirimliTutar = toplam - (toplam * indirimOranı / 100);
        int kdvTutar = indirimliTutar * kdv / 100;
        int finalTutar = indirimliTutar + kdvTutar;
        return new money(finalTutar, birimFiyat.ParaBirimi);
    }
}