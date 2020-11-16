using System;
using PudelkoLib;

namespace PudelkoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1 = new Pudelko(2, 2, 2);
            var p2 = new Pudelko(2, 2, 2);
            var p3 = p1 + p2;
            Console.WriteLine(p3.ToString());
        }
    }
}
