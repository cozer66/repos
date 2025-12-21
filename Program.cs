using System;
using System.Collections.Generic;
using System.IO;

namespace OtoparkYönetimSistemi
{
    class Program
    {
        static void Main(string[] args)
        {
            otopark myOtopark = new otopark(20);
            myOtopark.gözlemciEkle(new ParkYönetimi());
            myOtopark.gözlemciEkle(new dosyaloglama());

            bool devam = true;
            while (devam)
            {
                try
                {
                    Console.WriteLine("\n" + new string('~', 75));
                    Console.WriteLine("\n       AKILLI OTOPARK YÖNETİM SİSTEMİ ");
                    Console.WriteLine("\n" + new string('~', 75));

                    Console.WriteLine("\nOtopark Yönetim Sistemine Hoşgeldiniz!");
                    Console.WriteLine("\n1. Araç Ekle");
                    Console.WriteLine("2. Rezervasyon Yap");
                    Console.WriteLine("3. Araç Çıkar");
                    Console.WriteLine("4. Rapor Görüntüle");
                    Console.WriteLine("5. Log Dosyasını Göster");
                    Console.WriteLine("6. Çıkış");
                    Console.Write("Seçiminiz: ");

                    string seçim = Console.ReadLine();
                    switch (seçim)
                    {
                        case "1":
                            Console.Write("Araç Tipi (Araba, Motosiklet, Minibus, Tır, Otobüs , Kamyon, Kamyonet): ");
                            string aractipi = Console.ReadLine();
                            Arac Qarac = AracFabrikasi.Aracüret(aractipi);
                            Console.Write("Plaka: ");
                            Qarac.plaka = Console.ReadLine();

                            Console.Write("Ücret Türü (saatlik, günlük, abonelik): ");
                            string ücretTürü = Console.ReadLine().ToLower();
                            if (ücretTürü == "günlük")
                            {
                                Qarac.ucretStratejisi = new günlükücret();
                                Console.WriteLine("\n[BİLGİ] Günlük tarife seçildi, süre otomatik 24 saat olarak belirlendi.");
                            }
                            else if (ücretTürü == "saatlik") Qarac.ucretStratejisi = new saatlikücret();
                            else if (ücretTürü == "abonelik") Qarac.ucretStratejisi = new abonelikücret();
                            else
                            {
                                Console.WriteLine("Geçersiz ücret türü girdiniz. Varsayılan olarak saatlik ücret seçildi.");
                                Qarac.ucretStratejisi = new saatlikücret();
                            }


                            if (Qarac.ucretStratejisi is günlükücret) Qarac.süre = 24;
                            else
                            {
                                Console.Write("Süre (saat): ");
                                if (!int.TryParse(Console.ReadLine(), out int süre) || süre <= 0)
                                {
                                    Console.WriteLine("Geçersiz süre girdiniz.");
                                    Console.WriteLine("Devam etmek için bir tuşa basın...");
                                    Console.ReadKey();
                                    break;
                                }
                                Qarac.süre = süre;
                            }
                            if (myOtopark.aracEkle(Qarac))
                                Console.WriteLine("Araç başarıyla eklendi.");
                            else
                                Console.WriteLine("Otopark dolu, araç eklenemedi.");

                            break;
                        case "2":
                            Console.Write("Rezervasyon Yapılacak Yer Numarası (0 - 19): ");
                            int yerNumarası = int.Parse(Console.ReadLine());
                            var yerKontrol = myOtopark[yerNumarası];
                            if (yerKontrol != null)
                                throw new InvalidOperationException("Bu yer zaten dolu. Rezervasyon yapılamaz.");
                            else    {
                                Console.Write("Rezervasyon Süresi (saat): ");
                                int rezsüre = int.Parse(Console.ReadLine());
                                myOtopark.rezervasyonYap(yerNumarası, rezsüre);
                            }
                                break;
                        case "3":
                            Console.Write("Çıkarılacak Yer Numarası: ");
                            int çıkarYerNumarası = int.Parse(Console.ReadLine());
                            if (myOtopark[çıkarYerNumarası] == null)
                                throw new InvalidOperationException($"{çıkarYerNumarası} nolu yer zaten boş! Çıkarılacak bir araç bulunamadı.");
                            myOtopark[çıkarYerNumarası] = null;
                            Console.WriteLine("Araç başarıyla çıkarıldı.");
                            break;
                        case "4":
                            myOtopark.rapor();
                            break;
                        case "5":
                            myOtopark.dosyaloglamalistesi();
                            break;
                        case "6":
                            devam = false;
                            Console.WriteLine("Otopark Yönetim Sisteminden çıkılıyor. İyi günler!");
                            break;
                        default:
                            Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                            Console.WriteLine("Devam etmek için bir tuşa basın...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"\n[HATA] Giriş formatı hatalı: {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"\n[HATA] Geçersiz parametre: {ex.Message}");
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("\n[HATA] Geçersiz yer numarası girdiniz!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[HATA] {ex.Message}");
                }
                finally
                {
                    if (devam)
                    {
                        Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
    public interface IParkAlanı
    {
        void parkalangereksinimi();
    }
    public interface IUcret
    {
        double ucrethesapla(int süre, double fiyat);
    }

    public class saatlikücret : IUcret
    {
        public double ucrethesapla(int süre, double fiyat) => süre * fiyat;
    }
    public class günlükücret : IUcret
    {
        public double ucrethesapla(int süre, double fiyat) => 24 * fiyat;
    }
    public class abonelikücret : IUcret
    {
        public double ucrethesapla(int süre, double fiyat) => süre * fiyat * 0.8;
    }
    public interface Igözlemci
    {
        void bildirim(string mesaj);
    }
    public class ParkYönetimi : Igözlemci
    {
        public void bildirim(string mesaj)
        {
            Console.WriteLine($"\nPark Yönetimi Bildirimi: \n{mesaj}");
        }
    }
    public class dosyaloglama : Igözlemci
    {
        private string dosyaYolu = "otopark_log.json";
        public void bildirim(string mesaj)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(dosyaYolu, true))
                {
                    string logSatiri = $"{{\"tarih\": \"{DateTime.Now}\", \"olay\": \"{mesaj}\"}}";
                    sw.WriteLine(logSatiri);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dosyaya yazma hatası: " + ex.Message);
            }
        }

    }
    public abstract class Arac
    {
        public static int araçsayısı = 0;
        public Guid id { get; private set; } = Guid.NewGuid();
        public string plaka { get; set; }
        public int süre { get; set; }
        public IUcret ucretStratejisi { get; set; }
        public Arac()
        {
            ucretStratejisi = new saatlikücret();
        }
        public abstract double ucrethesapla();
        public abstract string parkalangereksinimi();

    }
    public class Otomobil : Arac
    {
        public override double ucrethesapla()
        {
            if (ucretStratejisi != null)
            {
                double fiyat = 60;
                double toplamUcret = ucretStratejisi.ucrethesapla(süre, fiyat);
                return toplamUcret;
            }
            return 0;
        }
        public override string parkalangereksinimi() => "5 m2";
    }
    public class Motosiklet : Arac
    {
        public override double ucrethesapla()
        {
            if (ucretStratejisi != null)
            {
                double fiyat = 30;
                double toplamUcret = ucretStratejisi.ucrethesapla(süre, fiyat);
                return toplamUcret;
            }
            return 0;
        }
        public override string parkalangereksinimi() => "2 m2";
    }
    public class Minibus : Arac
    {
        public override double ucrethesapla()
        {
            if (ucretStratejisi != null)
            {
                double fiyat = 90;
                double toplamUcret = ucretStratejisi.ucrethesapla(süre, fiyat);
                return toplamUcret;
            }
            return 0;
        }
        public override string parkalangereksinimi() => "7 m2";
    }
    public class tır : Arac
    {
        public override double ucrethesapla()
        {
            if (ucretStratejisi != null)
            {
                double fiyat = 150;
                double toplamUcret = ucretStratejisi.ucrethesapla(süre, fiyat);
                return toplamUcret;
            }
            return 0;
        }
        public override string parkalangereksinimi() => "12 m2";
    }
    public class otobüs : Arac
    {
        public override double ucrethesapla()
        {
            if (ucretStratejisi != null)
            {
                double fiyat = 120;
                double toplamUcret = ucretStratejisi.ucrethesapla(süre, fiyat);
                return toplamUcret;
            }
            return 0;
        }
        public override string parkalangereksinimi() => "9 m2";
    }
    public class kamyon : Arac
    {
        public override double ucrethesapla()
        {
            if (ucretStratejisi != null)
            {
                double fiyat = 130;
                double toplamUcret = ucretStratejisi.ucrethesapla(süre, fiyat);
                return toplamUcret;
            }
            return 0;
        }
        public override string parkalangereksinimi() => "10 m2";
    }
    public class kamyonet : Arac
    {
        public override double ucrethesapla()
        {
            if (ucretStratejisi != null)
            {
                double fiyat = 80;
                double toplamUcret = ucretStratejisi.ucrethesapla(süre, fiyat);
                return toplamUcret;
            }
            return 0;
        }
        public override string parkalangereksinimi() => "6 m2";
    }
    public static class AracFabrikasi
    {
        public static Arac Aracüret(string aractipi)
        {
            switch (aractipi.ToLower())
            {
                case "araba": return new Otomobil();
                case "motosiklet": return new Motosiklet();
                case "minibus": return new Minibus();
                case "tır": return new tır();
                case "otobüs": return new otobüs();
                case "kamyon": return new kamyon();
                case "kamyonet": return new kamyonet();
                default: throw new ArgumentException("Geçersiz araç tipi");
            }
        }
    }
    public class otopark
    {
        private Arac[] araçlar;
        private bool[] rezervasyonhali;
        private DateTime?[] tarihvesaat;
        private int?[] rezervasyonsüresi;

        private List<Igözlemci> gözlemciler = new List<Igözlemci>();
        public otopark(int kapasite)
        {
            araçlar = new Arac[kapasite];
            rezervasyonhali = new bool[kapasite];
            tarihvesaat = new DateTime?[kapasite];
            rezervasyonsüresi = new int?[kapasite];
        }
        public void gözlemciEkle(Igözlemci gözlemci)
        {
            gözlemciler.Add(gözlemci);
        }
        public void gözlemcibildirimi(string mesaj)
        {
            foreach (var gözlemci in gözlemciler)
            {
                gözlemci.bildirim(mesaj);
            }
        }
        public void rezervasyonYap(int index, int süre)
        {
            if (index < 0 && index >= araçlar.Length)
            {
               throw new IndexOutOfRangeException("Geçersiz yer numarası girdiniz!");
            }
            else if (!rezervasyonhali[index] && araçlar[index] == null)
            {
                rezervasyonhali[index] = true;
                tarihvesaat[index] = DateTime.Now;
                rezervasyonsüresi[index] = süre;
                gözlemcibildirimi($"{index} nolu yer {süre} saatliğine rezerve edildi.");
                gözlemcibildirimi($"Rezervasyon Tarihi ve Saati: {DateTime.Now}");
            }
            else 
            { 
                    Console.WriteLine("Bu yer dolu veya zaten rezerve edilmiş.");
            }
        }
        public Arac this[int index]
        {
            get
            {
                if (index >= 0 && index < araçlar.Length) return araçlar[index];
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index >= 0 && index < araçlar.Length)
                {
                    araçlar[index] = value;
                    if (value != null)
                    {
                        rezervasyonhali[index] = false;
                        tarihvesaat[index] = null;
                        gözlemcibildirimi($"{index} nolu yere {value.plaka} [ID: {value.id}] giriş yaptı.");
                    }
                    else
                    {
                        rezervasyonhali[index] = false;
                        tarihvesaat[index] = null;
                        gözlemcibildirimi($"{index} nolu yer boşaltıldı.");
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
        public bool aracEkle(Arac araç)
        {
            for (int i = 0; i < araçlar.Length; i++)
            {
                if (araçlar[i] == null)
                {
                    this[i] = araç;
                    return true;
                }
            }
            gözlemcibildirimi("Otopark dolu, araç eklenemedi.");
            return false;
        }
        public void rapor()
        {
            Console.WriteLine("\n" + new string('+', 75));
            Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-10}", "YER", "DURUM", "PLAKA", "TİP", "ALAN");
            Console.WriteLine(new string('+', 75));

            double toplamGelir = 0;
            int toplamAraç = 0;
            List<string> araçücretdetayları = new List<string>();


            for (int i = 0; i < araçlar.Length; i++)
            {
                if (araçlar[i] != null)
                {
                    double ucret = araçlar[i].ucrethesapla();
                    toplamGelir += ucret;
                    toplamAraç++;
                    araçücretdetayları.Add($"{araçlar[i].plaka} ({araçlar[i].GetType().Name}) Ücreti: {ucret} TL");
                    Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-10}", i, "DOLU", araçlar[i].plaka, araçlar[i].GetType().Name, araçlar[i].parkalangereksinimi());
                }
                else if (rezervasyonhali[i])
                {
                    Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-10}", i, "REZERVE", "-", "-", "-");
                }
                else
                {
                    Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-10}", i, "BOŞ", "-", "-", "-");
                }
            }
            Console.WriteLine("\n" + new string('¨', 50));
            if (araçücretdetayları.Count > 0)
            {
                Console.WriteLine("Araç Ücret Detayları:");
                foreach (var detay in araçücretdetayları)
                {
                    Console.WriteLine(detay);
                }
                Console.WriteLine(new string('¨', 50));
            }
            double dolulukOranı = (double)toplamAraç / araçlar.Length * 100;
            Console.WriteLine($"Doluluk Oranı: %{dolulukOranı:F2}");
            Console.WriteLine($"Toplam Araç Sayısı: {toplamAraç}");
            Console.WriteLine($"Toplam Gelir: {toplamGelir} TL");
            Console.WriteLine(new string('¨', 50) + "\n");
        }
        public void dosyaloglamalistesi()
        {
            string dosyaYolu = "otopark_log.json";
            if (File.Exists(dosyaYolu))
            {
                Console.WriteLine("\n--- SİSTEM LOG KAYITLARI (JSON) ---");
                string[] satirlar = File.ReadAllLines(dosyaYolu);
                foreach (var satir in satirlar)
                {
                    Console.WriteLine(satir);
                }
            }
            else
            {
                Console.WriteLine("Henüz bir log dosyası oluşturulmamış.");
            }
        }
    }
}
