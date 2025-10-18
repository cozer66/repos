using System;

// Ana program
otobüsyönetimi otobüs = new otobüsyönetimi();
Console.WriteLine("--- 1. BİNİŞ İŞLEMLERİ (Kapasite 50) ---");
otobüs.yolcual("Ali", "Veli", "12345", "durak A", "durak C");
otobüs.yolcual("Ayşe", "Fatma", "67890", "durak B", "durak D");

Console.WriteLine("\n--- 2. SENARYO 50 DEN FAZLA YOLCU ---");
otobüs.Kapasiteyidoldur();

otobüs.yolcual("Zeynep", "Yılmaz", "11223", "durak A", "durak F");
otobüs.yolcual("Ahmet", "Kaya", "44556", "durak B", "durak E");

otobüs.beklemelistesiGoster();
Console.WriteLine("\n--- Mevcut Yolcular ---");
otobüs.yolculistesiGoster();

Console.WriteLine("\n--- 3. SENARYO İNİŞ İŞLEMLERİ ---");
otobüs.yolcucikar("12345"); // Ali Veli iniyor
otobüs.yolculistesiGoster();
otobüs.beklemelistesiGoster();

Console.WriteLine("\n--- Beklemeden Alım ---");
otobüs.beklemedenal(null); // Bekleme listesinden yolcu alınıyor
otobüs.yolculistesiGoster();
otobüs.beklemelistesiGoster();

otobüs.yolcual("Mehmet", "Demir", "33445", "durak C", "durak G"); // Yeni yolcu ekleniyor

Console.WriteLine("\n--- 4. SENARYO İKİNCİ BİNİŞ --- ");
otobüs.yolcucikar("67890"); // Ayşe Fatma iniyor
otobüs.beklemedenal(null); // Bekleme listesinden yolcu alınıyor
otobüs.yolculistesiGoster();
otobüs.beklemelistesiGoster();
otobüs.yolcual("Elif", "Çelik", "55667", "durak D", "durak H"); // Yeni yolcu ekleniyor

Console.WriteLine("\n--- 5. HATA TESTİ ---");
otobüs.yolcucikar("00000"); // Geçersiz kart numarası


public class yolcubilgileri// yolcu bilgileri sınıfı
{
    public string ad { get; set; }
    public string soyad { get; set; }
    public string kartno { get; set; }
    public string binis { get; set; }
    public string inis { get; set; }

    public yolcubilgileri sıradaki { get; set; }

    public yolcubilgileri(string ad, string soyad, string kartno, string binis, string inis)// constructor metodu
    {
        this.ad = ad;
        this.soyad = soyad;
        this.kartno = kartno;
        this.binis = binis;
        this.inis = inis;
        this.sıradaki = null;
    }
    public override string ToString()// yolcu bilgilerini string olarak döndüren metot
    {
        return $"{ad} {soyad} - Kart No: {kartno} - Biniş: {binis} - İniş: {inis}";
    }
}
public class otobüsyönetimi// otobüs yönetimi sınıfı
{
    private static int globalbeklemeSırasayacı = 0; // bekleme sırasını tutan statik değişken
    private yolcubilgileri yolculistesi;
    private yolcubilgileri beklemelistesi;
    private static int kapasite = 50;
    private int mevcutYolcu = 0;
    private int beklemeSırası = 0;

    public otobüsyönetimi()// constructor metodu
    {
        yolculistesi = null;
        beklemelistesi = null;
    }
    
    public void yolcual(string ad, string soyad, string kartno, string binis, string inis = null)// yolcu ekleme metodu
    {
        yolcubilgileri neyiYolcu = new yolcubilgileri(ad, soyad, kartno, binis, inis); // yeni yolcu oluştur
        //yeni yazacağıma neyi yazmışım sonrasında değiştirmedim

        if (mevcutYolcu < kapasite)// otobüste yer varsa yolcuyu ekle
        {
            yolculistesi =listekle(this.yolculistesi, neyiYolcu);
            mevcutYolcu++;
            Console.WriteLine($"{ad} {soyad} otobüse alındı.Mevcut yolcu sayısı: {mevcutYolcu}/{kapasite}");
        }
        else// otobüs doluysa bekleme listesine ekle
        {
            beklemelistesi = listekle(beklemelistesi, neyiYolcu);
            beklemeSırası++;
            globalbeklemeSırasayacı++;
            Console.WriteLine($"{ad} {soyad} bekleme listesine alındı.Sıra No: {beklemeSırası}, Global Sıra No: {globalbeklemeSırasayacı}");
        }
    }
    private yolcubilgileri listekle(yolcubilgileri liste, yolcubilgileri yeniYolcu)// listeye yolcu ekleme metodu
    {
        if (liste == null)// liste boşsa yeni yolcuyu başa ekle
        {
            return yeniYolcu;
        }
        yolcubilgileri temp = liste;
        while (temp.sıradaki != null)// liste sonuna kadar ilerle
        {
            temp = temp.sıradaki;
        }
        temp.sıradaki = yeniYolcu;
        return liste;
    }
    public void yolcucikar(string kartno)// yolcu çıkarma metodu
    {
        yolcubilgileri geçici = yolculistesi;
        yolcubilgileri önceki = null;
        while (geçici != null)// yolcu listesini dolaşarak kart numarasını ara
        {
            if (geçici.kartno == kartno)
            {
                if (önceki == null)
                {
                    yolculistesi = geçici.sıradaki;
                }
                else
                {
                    önceki.sıradaki = geçici.sıradaki;
                }
                mevcutYolcu--;
                Console.WriteLine($"{geçici.ad} {geçici.soyad} {geçici.inis} durağında otobüsten indi.Mevcut yolcu sayısı: {mevcutYolcu}/{kapasite}");
                return;
            }
            önceki = geçici;
            geçici = geçici.sıradaki;

        }
        Console.WriteLine("Geçersiz kart numarası, yolcu bulunamadı.");

    }

    public void beklemedenal(yolcubilgileri yolcubilgileri)// bekleme listesinden yolcu al metodu
    {
        if (beklemelistesi != null && mevcutYolcu < kapasite)// bekleme listesinde yolcu varsa ve otobüste yer varsa
        {
            yolcubilgileri yeniYolcu = beklemelistesi;
            beklemelistesi = beklemelistesi.sıradaki;
            yeniYolcu.sıradaki = null;
            beklemeSırası--;
            yolculistesi = listekle(yolculistesi, yeniYolcu);
            mevcutYolcu++;
            Console.WriteLine($"{yeniYolcu.ad} {yeniYolcu.soyad} bekleme listesinden otobüse alındı.Mevcut yolcu sayısı: {mevcutYolcu}/{kapasite}");
        }

    }
    public void Kapasiteyidoldur()
    {
        mevcutYolcu = kapasite;
        Console.WriteLine("mevcut yolcu sayısı: 50/50");
    } // kapasiteyi 50 yapar ve döner
    public void yolculistesiGoster()// yolcu listesini gösteren metot
    {

        if (yolculistesi == null)// yolcu listesi boşsa mesaj ver
        {
            Console.WriteLine("Otobüs boş.");
            return;
        }
        Console.WriteLine("Otobüsteki Yolcular:");
        yolcubilgileri temp = yolculistesi;
        while (temp != null)
        {
            Console.WriteLine($"{temp.ad} {temp.soyad} - Kart No: {temp.kartno} - Biniş: {temp.binis}");
            temp = temp.sıradaki;
        }
        while (temp != null) { temp = temp.sıradaki; }// temp'ı sonuna kadar ilerlet

    }
    public void beklemelistesiGoster() // bekleme listesini gösteren metot
    {
        if (beklemelistesi == null)// bekleme listesi boşsa mesaj ver
        {
            Console.WriteLine("Bekleme listesi boş.");
            return;
        }
        Console.WriteLine("Bekleme Listesindeki Yolcular:");
        yolcubilgileri temp = beklemelistesi;
        while (temp != null) // temp'ı sonuna kadar ilerlet
        {
            Console.WriteLine($"{temp.ad} {temp.soyad} - Kart No: {temp.kartno} ");
            temp = temp.sıradaki;
        }
    }
    public int mevcutYolcuSayisi() // mevcut yolcu sayısını döndüren metot
    {
        return mevcutYolcu;
    }
    public int beklemeSırasıSayisi() // bekleme sırası sayısını döndüren metot
    {
        return beklemeSırası;
    }

}

