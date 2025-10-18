
ogrenci ogrenci1 = new ogrenci();
ogrenci1.ad = "Ali Veli";
Console.WriteLine("Öğrenci Adı: " + ogrenci1.ad);

ogrenci1.ad = ""; // Hata mesajı verir: İsim boş olamaz.

Console.WriteLine("\n------");
ogrenci1.yazdir(); // Öğrenci Adı: Ali Veli

public class ogrenci
{
    private string isim;
    public string ad
    {
        get {  return isim; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                Console.WriteLine("\nİsim boş olamaz.");
            }
            else
            {
                isim = value;
            }
        }
    }
    public void yazdir()
    {
        Console.WriteLine("Öğrenci Adı: " + isim);
    }
}