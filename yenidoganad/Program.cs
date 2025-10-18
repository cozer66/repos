
insan cocuk = new insan("ali", "yılmaz"); 
Console.WriteLine("çocuk bilgileri:");
cocuk.bilgiver();

Console.WriteLine("\nçocuk bilgileri güncelleniyor...");
cocuk.name = "veli"; //name değiştirilebilir 
cocuk.bilgiver();
public class insan
{
    
    private readonly string _surname;
    public string surname
    {
        get { return _surname; }
    }
    public string name { get; set; }
    public insan(string name, string fathersurname)
    {
        this.name = name;
        this._surname = fathersurname;
    }
    public void bilgiver()
    {
        Console.WriteLine($"adı:{name}");
        Console.WriteLine($"soyadı:{surname}");
    }
}
