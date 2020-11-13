using System;
using PudelkoLib;

namespace PudelkoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var pudelko = new Pudelko(2, 11, 3.05, jednostkaMiary: UnitOfMeasure.Millimeter);
            Pudelko p = new Pudelko(jednostkaMiary: UnitOfMeasure.Millimeter, dlugosc: 100, szerokosc: 25.58, wysokosc: 3.13);
            Console.WriteLine(p.ToString());
            Console.WriteLine(p.ToString("mm"));
            Console.WriteLine(p.ToString("cm"));
            //a: 11, expectedA: 0.011, b: 2599 => 2.599
            //var pudelko2 = new Pudelko(2.1, 3.05, 1);
        }
    }
}
