using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PudelkoLib
{
    public sealed class Pudelko : IFormattable
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
            if(IsExceedingBoxSize(dlugosc, szerokosc, wysokosc, jednostkaMiary))
                throw new ArgumentOutOfRangeException();
            

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

        public override string ToString()
        {
            return ToString("m", CultureInfo.CurrentCulture);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            if(string.IsNullOrEmpty(format))
                throw new FormatException("Invalid string format");
            if(provider == null)
                provider = CultureInfo.CurrentCulture;

            var unitFormat = "";
            var dlugosc = 0.0;
            var szerokosc = 0.0;
            var wysokosc = 0.0;
            switch (format)
            {
                case "m":
                    unitFormat = "0.000";
                    dlugosc = UnitConverter.ToMeter(_dlugosc, _jednostkaMiary);
                    szerokosc = UnitConverter.ToMeter(_szerokosc, _jednostkaMiary);
                    wysokosc = UnitConverter.ToMeter(_wysokosc, _jednostkaMiary);
                    break;
                case "cm":
                    unitFormat = "##0.0";
                    dlugosc = UnitConverter.ToCentimeter(_dlugosc, _jednostkaMiary);
                    szerokosc = UnitConverter.ToCentimeter(_szerokosc, _jednostkaMiary);
                    wysokosc = UnitConverter.ToCentimeter(_wysokosc, _jednostkaMiary);
                    break;
                case "mm":
                    unitFormat = "####";
                    dlugosc = UnitConverter.ToMillimeter(_dlugosc, _jednostkaMiary);
                    szerokosc = UnitConverter.ToMillimeter(_szerokosc, _jednostkaMiary);
                    wysokosc = UnitConverter.ToMillimeter(_wysokosc, _jednostkaMiary);
                    break;
            }

            return $"{dlugosc.ToString(unitFormat, provider)} {format} × {szerokosc.ToString(unitFormat, provider)}" +
                   $" {format} × {wysokosc.ToString(unitFormat, provider)} {format}";
        }
    }
}
