using System;
using System.Collections.Generic;
using System.Text;
using PudelkoLib;

namespace PudelkoApp
{
    public static class Extensions
    {
        public static Pudelko Kompresuj(this Pudelko pudelko)
        {
            var volume = pudelko.Objetosc;
            var edge = Math.Pow(volume, 1.0 / 3.0);
            return new Pudelko(edge, edge, edge);
        }
    }
}
