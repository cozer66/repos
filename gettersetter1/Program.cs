ogrenci ogrenci1 = new ogrenci(); // Create an instance of the ogrenci class
ogrenci1.setAd("Ahmet"); // Set the name of the student

string ad = ogrenci1.getAd(); 
Console.WriteLine("Öğrenci Adı: " + ad); 

Console.WriteLine("Öğrenci Adı:");
ogrenci1.setAd(""); 
ogrenci1.Yazdir(); 

Console.WriteLine("\n------");
ogrenci1.Yazdir(); 

public class ogrenci { 
private string ad;

    public void setAd(string ad) { 
        if (string.IsNullOrEmpty(ad)) { 
            Console.WriteLine("ad boş olamaz!");
        }
        else { 
            this.ad = ad;
        }
    }
    public string getAd() { 
        return ad;
    }
    public void Yazdir() { 
        Console.WriteLine("Öğrenci Adı: " + ad);
    }
}