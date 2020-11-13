using System;
using PudelkoLib;

namespace PudelkoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var pudelko = new Pudelko(2, 11, 3.05, jednostkaMiary: UnitOfMeasure.Millimeter);
            var pudelko2 = new Pudelko(2, 11, 3.05, jednostkaMiary: UnitOfMeasure.Millimeter);
            Console.WriteLine(pudelko.Equals(pudelko2));
            Console.WriteLine(pudelko[0]);
            Console.WriteLine(pudelko[1]);
            Console.WriteLine(pudelko[2]);
            Console.WriteLine("=====");
            foreach(var wymiar in pudelko)
                Console.WriteLine(wymiar);
            
            //a: 11, expectedA: 0.011, b: 2599 => 2.599
            //var pudelko2 = new Pudelko(2.1, 3.05, 1);
        }
    }
}
