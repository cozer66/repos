
using System;

// 1. Soyut Temel Sınıf (Abstract Base Class)
// 'abstract' sınıfından doğrudan nesne oluşturulamaz, sadece miras alınabilir.
public abstract class Calisan
{
    public string AdSoyad { get; set; }
    public decimal Maas { get; private set; }

    public Calisan(string adSoyad, decimal maas)
    {
        this.AdSoyad = adSoyad;
        this.Maas = maas;
    }

    // Soyut Metot: Sadece imzası (tanımı) vardır, gövdesi yoktur.
    // Miras alan her sınıf bu metodu (override ile) ZORUNLU olarak uygulamalıdır.
    public abstract void GorevYap();

    // Normal Metot (Tüm çalışanlar kullanabilir)
    public void MaasGoster()
    {
        Console.WriteLine($"{AdSoyad}'ın maaşı: {Maas:C}");
    }
}

// 2. Birinci Düzey Türetilmiş Sınıf (Concrete Class - Somut Sınıf)
public class Programci : Calisan
{
    public string UzmanlikAlani { get; set; }

    public Programci(string adSoyad, decimal maas, string uzmanlik) : base(adSoyad, maas)
    {
        this.UzmanlikAlani = uzmanlik;
    }

    // ZORUNLU Uygulama: Temel sınıftaki soyut metot implemente edilmelidir.
    public override void GorevYap()
    {
        Console.WriteLine($"{AdSoyad} ({UzmanlikAlani}): Kod yazıyor ve hataları ayıklıyor.");
    }
}

// 3. Çok Katmanlı Kalıtım (Programci'dan miras alma)
// UzmanProgramci, Calisan'dan (dolaylı olarak) ve Programci'dan miras alır.
public class UzmanProgramci : Programci
{
    public int ProjeSayisi { get; set; }

    // Yapıcıda 3 seviye yukarıdaki Calisan'a kadar değerleri taşıyoruz.
    public UzmanProgramci(string adSoyad, decimal maas, string uzmanlik, int projeSayisi)
        : base(adSoyad, maas * 1.5M, uzmanlik) // Uzman olduğu için maaşı %50 artırdık.
    {
        this.ProjeSayisi = projeSayisi;
    }

    // Programcı'nın metodunu daha özel bir şekilde ezme
    public override void GorevYap()
    {
        // Temel (Programci) sınıfın metodunu da çağırabiliriz.
        base.GorevYap();
        Console.WriteLine($"Ek olarak, {ProjeSayisi} büyük projeye liderlik ediyor.");
    }
}

// Kullanım
public class Program3
{
    public static void Main(string[] args)
    {
        // Calisan tipinde UzmanProgramci nesnesi oluşturulabilir (Polymorphism)
        Calisan uzman = new UzmanProgramci("Ayşe Yılmaz", 50000, "C#/.NET", 5);

        uzman.MaasGoster(); // Calisan'dan miras alınan metot
        uzman.GorevYap();   // Çalışanın gerçek tipine (UzmanProgramci) özel metot

        // Calisan yeniCalisan = new Calisan("isim", 1000); // HATA: Soyut sınıftan nesne oluşturulamaz.
    }
}

