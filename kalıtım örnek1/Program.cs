using System;

// Temel Sınıf
public class Sekil
{
    // Sanal (virtual) metot: Alt sınıfların bu metodu kendilerine göre değiştirmesine izin verir.
    public virtual double AlanHesapla()
    {
        return 0; // Varsayılan değer, genel bir şekil için alan 0'dır.
    }
}

// Türetilmiş Sınıf 1
public class Daire : Sekil
{
    public double Yaricap { get; set; }

    public Daire(double yaricap)
    {
        this.Yaricap = yaricap;
    }

    // override: Temel sınıftaki AlanHesapla metodunu kendi kurallarımızla eziyoruz (geçersiz kılıyoruz).
    public override double AlanHesapla()
    {
        return Math.PI * Yaricap * Yaricap;
    }
}

// Türetilmiş Sınıf 2
public class Kare : Sekil
{
    public double KenarUzunlugu { get; set; }

    public Kare(double kenar)
    {
        this.KenarUzunlugu = kenar;
    }

    // override: Temel sınıftaki AlanHesapla metodunu kendi kurallarımızla eziyoruz.
    public override double AlanHesapla()
    {
        return KenarUzunlugu * KenarUzunlugu;
    }
}

// Kullanım
public class Program2
{
    public static void Main(string[] args)
    {
        // Çok Biçimlilik: Farklı nesneleri (Daire, Kare) ortak bir tipte (Sekil) tutabilme yeteneği.
        Sekil[] sekiller = new Sekil[2];
        sekiller[0] = new Daire(5); // Sekil tipinde Daire nesnesi tutuluyor.
        sekiller[1] = new Kare(4);  // Sekil tipinde Kare nesnesi tutuluyor.

        foreach (Sekil s in sekiller)
        {
            // Metodu çağırdığımızda, nesnenin gerçek tipi (Daire veya Kare) baz alınır.
            // Bu, Polymorphism'dir.
            Console.WriteLine($"Şeklin Alanı: {s.AlanHesapla():F2}");
        }
        // Sonuç: Daire için (3.14 * 25) ve Kare için (4 * 4) hesaplanır.
    }
}