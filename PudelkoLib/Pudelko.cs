using System;

namespace PudelkoLib
{
    public sealed class Pudelko
    {
        private readonly double _dlugosc;
        private readonly double _szerokosc;
        private readonly double _wysokosc;
        private readonly UnitOfMeasure _jednostkaMiary;

        public double Dlugosc => Math.Round(UnitConverter.ToMeter(_dlugosc, _jednostkaMiary), 3);
        public double Szerokosc => Math.Round(UnitConverter.ToMeter(_szerokosc, _jednostkaMiary), 3);
        public double Wysokosc => Math.Round(UnitConverter.ToMeter(_wysokosc, _jednostkaMiary), 3);

        public Pudelko(double dlugosc = 0.1, double szerokosc = 0.1, double wysokosc = 0.1, 
            UnitOfMeasure jednostkaMiary = UnitOfMeasure.Meter)
        {
            if (IsExceedingBoxSize(dlugosc, szerokosc, wysokosc, jednostkaMiary))
            {
                throw new ArgumentOutOfRangeException();
            }

            _dlugosc = dlugosc;
            _szerokosc = szerokosc;
            _wysokosc = wysokosc;
            _jednostkaMiary = jednostkaMiary;
        }

        private bool IsExceedingBoxSize(double dlugosc, double szerokosc, double wysokosc, UnitOfMeasure jednostkaMiary)
        {
            if(dlugosc <= 0 || szerokosc <= 0 || wysokosc <= 0)
                return true;
            if (jednostkaMiary == UnitOfMeasure.Meter && (dlugosc > 10.0 || szerokosc > 10.0 || wysokosc > 10.0))
                return true;
            if  (jednostkaMiary == UnitOfMeasure.Centimeter && (dlugosc > 1000.0 || szerokosc > 1000.0 || wysokosc > 1000.0))
                return true;
            if (jednostkaMiary == UnitOfMeasure.Millimeter && (dlugosc > 10000.0 || szerokosc > 10000.0 || wysokosc > 10000.0))
                return true;

            return false;
        }

    }
}
