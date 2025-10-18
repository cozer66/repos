using System;
yenidogan yenidogan = new yenidogan("Ali", "Yilmaz", 1234678901);
yenidogan.BilgileriGoster();

yenidogan yenidogan1 = new yenidogan("Ayse", "Kara", 987654320);
yenidogan1.BilgileriGoster();

Console.WriteLine("\nAd degistiriliyor...");
yenidogan1.Ad = "Fatma";
yenidogan1.BilgileriGoster();

Console.WriteLine("\nTcNo degistiriliyor...");

Console.WriteLine("Eski Tc:"+ yenidogan1.TcNo);
yenidogan1.TcNo = 1122334455; // Tek sayi oldugu icin 0 olarak gosterilecek
Console.WriteLine("Yeni Tc:"+ yenidogan1.TcNo);

Console.WriteLine("\nBilgiler güncellerniyor...");
yenidogan1.BilgileriGoster();




public class yenidogan
{
    private readonly string soyad;
    private string isim;
    private int tcNo;

    public yenidogan(string isim, string soyad, int tcNo)
    {
        this.isim = isim;
        this.soyad = soyad;
        this.tcNo = tcNo;
    }
    public string Ad
    {
        // 4. Kural: Adı sorunca tamamını söyler.
        get { return isim; }
        
        // 2. Kural: Doğduktan sonra değiştirilebilir.
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ad boş olamaz.");
            isim = value.Trim();
        }
    }
    public string Soyad
    {
        // 3. Kural: Soyadı sorunca tamamını söyler.
        get {
        char ilkHarf = char.ToUpper(soyad[0]);
        char sonadHarf = char.ToLower(soyad[soyad.Length - 1]);
        string gizliKisim = new string('*', soyad.Length - 2);
            return $"{ilkHarf}{gizliKisim}{sonadHarf}";
        }
    }
    public int TcNo
    {
        // 1. Kural: TC numarasını sorunca sadece son 2 hanesini söyler.
        get
        {
            if (tcNo % 2 == 0)
                return tcNo;
            else
                return 0;
        }
        set 
        {        
            if (value <= 0)
                throw new ArgumentException("TcNo pozitif bir sayi olmalidir", nameof(value));
            tcNo = value;
        }
    }
    public void BilgileriGoster()
    {
        Console.WriteLine($"\nAd: {Ad}");
        Console.WriteLine($"Soyad: {Soyad}");
        Console.WriteLine($"TcNo: {TcNo}");
    }
    }