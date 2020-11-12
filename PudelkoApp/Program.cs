using System;
using PudelkoLib;

namespace PudelkoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var pudelko = new Pudelko(2.5, 9.321, 0.1, UnitOfMeasure.Meter);
            Console.WriteLine(pudelko.ToString("mm"));
        }
    }
}
