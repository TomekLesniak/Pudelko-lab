﻿using System;
using PudelkoLib;

namespace PudelkoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var pudelko = new Pudelko(3, 3, 3, jednostkaMiary: UnitOfMeasure.Meter);
            var pudelko2 = new Pudelko(1, 3, 3, jednostkaMiary: UnitOfMeasure.Meter);

            var parsed = Pudelko.Parse("2,500 m × 9,321 m × 0,100 m");
            Console.WriteLine(parsed.ToString());

            var x = pudelko + pudelko2;
            Console.WriteLine(x.Pole);

            //a: 11, expectedA: 0.011, b: 2599 => 2.599
            //var pudelko2 = new Pudelko(2.1, 3.05, 1);
        }
    }
}
