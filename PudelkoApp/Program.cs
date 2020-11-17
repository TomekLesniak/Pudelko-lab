using System;
using System.Collections.Generic;
using System.Diagnostics;
using PudelkoLib;

namespace PudelkoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var test1 = new Pudelko(10,5,5);
            var test2 = new Pudelko(7,5,1);
            Console.WriteLine($"{test1.ToString()} == {test2.ToString()} : {test1 == test2}");
            Console.WriteLine($"{test1.ToString()} != {test2.ToString()} : {test1 != test2}");
            var test3 = test1 + test2;
            Console.WriteLine($"Po dodaniu: {test3.ToString()}");
            Console.WriteLine($"W centymetrach: {test3.ToString("cm")}");
            Console.WriteLine($"W milimetrach: {test3.ToString("mm")}");

            Console.WriteLine("============\nKonwersja");

            double[] test3Arr = test3;
            Console.WriteLine($"Jawna: Typ zwracany => {test3Arr}");

            var test4 = (Pudelko) (1000, 2000, 3000);
            Console.WriteLine($"Niejawna: W milimetrach => {test4.ToString("mm")}");


            Console.WriteLine("============\nIndexer");

            var indexer = new Pudelko(0.1,1,2);

            Console.WriteLine($"Index 0 => {indexer[0]}");
            Console.WriteLine($"Index 1 => {indexer[1]}");
            Console.WriteLine($"Index 2 => {indexer[2]}");
            try
            {
                Console.WriteLine(indexer[3]);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Index 3 => {ex}");
            }

            Console.WriteLine("============");



            var toCompress = new Pudelko(2, 3, 4);
            var afterCompress = toCompress.Kompresuj();


            Console.WriteLine($"Przed kompresja: objetosc = {toCompress.Volume}, wymiary = {toCompress.ToString()}");
            Console.WriteLine($"Po kompresji: objetosc = {Math.Round(afterCompress.Volume, 3)}, wymiary = {afterCompress.ToString()}");



            Console.WriteLine("============");

            var p0 = new Pudelko();
            var p1 = new Pudelko(1);
            var p2 = new Pudelko(2, 3);
            var p3 = new Pudelko(400, 100, 500, UnitOfMeasure.Centimeter);
            var p4 = new Pudelko(9000, 1000, 3000, UnitOfMeasure.Millimeter);
            var p5 = new Pudelko(10, 10, 10);
            var p6 = new Pudelko(3, 3, 3);
            var p7 = new Pudelko(1, 5, 10);
            var p8 = new Pudelko(2, 5, 5);
            var p9 = p7 + p8;
            var p10 = Pudelko.Parse("5 m × 3 m × 2 m");

            var boxes = new List<Pudelko> {p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10};

            Console.WriteLine("NIEPOSORTOWANE:");
            foreach (var box in boxes)
            {
                Console.WriteLine(box.ToString());
            }

            boxes.Sort(((box1, box2) =>
            {
                if (Math.Abs(box1.Volume - box2.Volume) > 0.001)
                    return box1.Volume > box2.Volume ? 1 : -1;
                if (Math.Abs(box1.Area - box2.Area) > 0.001)
                    return box1.Area > box2.Area ? 1 : -1;

                return box1.A + box1.B + box1.C > box2.A + box2.B + box2.C
                    ? 1
                    : -1;

            }));

            Console.WriteLine("=============\nPOSORTOWANE:");

            foreach (var box in boxes)
            {
                Console.WriteLine(box.ToString() + " obj: " + box.Volume + " pole: " + box.Area);
            }
        }
    }
}
