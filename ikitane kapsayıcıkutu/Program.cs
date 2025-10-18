
box box1 = new box(3, 4, 5);
box box2 = new box(6, 7, 8);
box box3 = box1 + box2;


Console.WriteLine("toplam yükseklik: " + box3.V1);
Console.WriteLine("toplam genişlik: " + box3.V2);
Console.WriteLine("toplam derinlik: " + box3.V3);

public class box
{
    private double yükseklik;
    private double genişlik;
    private double derinlik;

    public box(int yükseklik, int genişlik, int derinlik)
    {
        this.yükseklik = yükseklik;
        this.genişlik = genişlik;
        this.derinlik = derinlik;
    }

    public box(double v1, double v2, double v3)
    {
        V1 = v1;
        V2 = v2;
        V3 = v3;
    }

    public double V1 { get; }
    public double V2 { get; }
    public double V3 { get; }

    public static box operator +(box b1, box b2)
    {

        double newYükseklik = b1.yükseklik + b2.yükseklik;
        double newGenişlik = b1.genişlik + b2.genişlik;
        double newDerinlik = b1.derinlik + b2.derinlik;
        return new box(newYükseklik, newGenişlik, newDerinlik);
    }
}