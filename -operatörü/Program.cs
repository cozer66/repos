calculter c = new calculter(80, 20);

c.print();
Console.WriteLine(-c); // operator overloading

int fark = -c; // operator overloading
Console.WriteLine($"fark={fark}");

public class calculter
{
    private int a, b;
    public calculter(int a, int b)
    {
        this.a = a;
        this.b = b;
    }
    public void print()
    {
        Console.WriteLine($"a={a}, b={b}");
    }
    public static int operator -(calculter c)
    {
        return c.a - c.b;
    }
}