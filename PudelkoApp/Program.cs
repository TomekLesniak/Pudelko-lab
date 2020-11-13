using System;
using PudelkoLib;

namespace PudelkoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var pudelko = new Pudelko(2, 2, 3.05);
            //var pudelko2 = new Pudelko(2.1, 3.05, 1);
            var pudelko2 = new Pudelko(3.05, 2,2);
            Console.WriteLine(pudelko.ToString("mm"));
            Console.WriteLine(pudelko.Equals(pudelko2));
        }
    }
}
