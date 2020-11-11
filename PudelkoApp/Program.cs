using System;
using PudelkoLib;

namespace PudelkoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var pudelko = new Pudelko(159, 100.0, 100.0, UnitOfMeasure.Centimeter);
            Console.WriteLine(pudelko.Dlugosc);
            Console.WriteLine(pudelko.Szerokosc);
            Console.WriteLine(pudelko.Wysokosc);
        }
    }
}
