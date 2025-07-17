
using System;

public interface IGreeter
{
    void Greet();
}

public class Greeter : IGreeter
{
    public void Greet()
    {
        Console.WriteLine("Hello, World!");
    }
}
