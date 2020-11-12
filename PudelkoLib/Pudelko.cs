using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Console = System.Console;

namespace PudelkoLib
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>
    {
        private readonly double _dlugosc;
        private readonly double _szerokosc;
        private readonly double _wysokosc;
        private readonly UnitOfMeasure _jednostkaMiary;

        public double Dlugosc => Math.Round(UnitConverter.ToMeter(_dlugosc, _jednostkaMiary), 3);
        public double Szerokosc => Math.Round(UnitConverter.ToMeter(_szerokosc, _jednostkaMiary), 3);
        public double Wysokosc => Math.Round(UnitConverter.ToMeter(_wysokosc, _jednostkaMiary), 3);
        public double Objetosc => Math.Round(Dlugosc * Szerokosc * Wysokosc, 9);

        public double Pole =>
            Math.Round((2 * Dlugosc * Szerokosc) + (2 * Dlugosc * Wysokosc) + (2 * Szerokosc * Wysokosc), 6);

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

        public bool Equals(Pudelko other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            //todo: Sprawdzic czy boki sa identyczne
            return false;

        }

        public override bool Equals(object obj)
        {
            if (obj is Pudelko)
                return Equals((Pudelko) obj);

            return false;
        }

        public static bool Equals(Pudelko p1, Pudelko p2)
        {
            if (p1 == null && p2 == null)
                return true;
            if (p1 is null) 
                return false;

            return p1.Equals(p2);
        }

        public static bool operator ==(Pudelko p1, Pudelko p2) => Equals(p1, p2);
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);

        public override int GetHashCode()
        {
            return (_dlugosc, _szerokosc, _wysokosc, _jednostkaMiary).GetHashCode();
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
