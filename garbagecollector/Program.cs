using System;


complex c = new complex();
c.putdata();

c.getdata();
c.putdata();

complex c1 = new complex(5, 6);
c1.putdata();
c = null;

Console.WriteLine("Forcing Garbage Collection");
GC.Collect();
GC.WaitForPendingFinalizers();
Console.WriteLine("End of Main");

public class complex
{
    private int real, imag;
    public complex(int r = 0, int i = 0) { this.real = r; this.imag = i; }
    public void getdata()
    {
        Console.WriteLine("Enter real and imaginary part: ");
        real = Convert.ToInt32(Console.ReadLine());
        imag = Convert.ToInt32(Console.ReadLine());
    }
    public void putdata() { Console.WriteLine("The complex number is: {0}+{1}i", real, imag); }

    public void display()
    {
        throw new NotImplementedException();
    }
    ~complex() 
    { 
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Destructor called"); 
        Console.ResetColor();
    }

}

