using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace PudelkoLib
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        private readonly double _dlugosc;
        private readonly double _szerokosc;
        private readonly double _wysokosc;
        private readonly UnitOfMeasure _jednostkaMiary;

        public double Dlugosc => Math.Round(_dlugosc, 4);
        public double Szerokosc => Math.Round(_szerokosc, 4);
        public double Wysokosc => Math.Round(_wysokosc, 4);
        public double Objetosc => Math.Round(Dlugosc * Szerokosc * Wysokosc, 9);
        public double Pole =>
            Math.Round((2 * Dlugosc * Szerokosc) + (2 * Dlugosc * Wysokosc) + (2 * Szerokosc * Wysokosc), 6);

        public Pudelko()
        {
            _dlugosc = 0.1;
            _szerokosc = 0.1;
            _wysokosc = 0.1;
            _jednostkaMiary = UnitOfMeasure.Meter;
        }

        public Pudelko(double dlugosc, UnitOfMeasure jednostkaMiary = UnitOfMeasure.Meter) : this()
        {
            _dlugosc = UnitConverter.ToMeter(dlugosc, jednostkaMiary);
            if (IsExceedingBoxSize(_dlugosc))
                throw new ArgumentOutOfRangeException();

            _jednostkaMiary = jednostkaMiary;
        }

        public Pudelko(double dlugosc, double szerokosc, UnitOfMeasure jednostkaMiary = UnitOfMeasure.Meter) : this(dlugosc, jednostkaMiary)
        {
            _szerokosc = UnitConverter.ToMeter(szerokosc, jednostkaMiary);
            if (IsExceedingBoxSize(_szerokosc))
                throw new ArgumentOutOfRangeException();
        }

        public Pudelko(double dlugosc , double szerokosc, double wysokosc,
            UnitOfMeasure jednostkaMiary = UnitOfMeasure.Meter) : this(dlugosc, szerokosc, jednostkaMiary)
        {
            _wysokosc = UnitConverter.ToMeter(wysokosc, jednostkaMiary);
            if (IsExceedingBoxSize(_wysokosc))
                throw new ArgumentOutOfRangeException();
        }

        private bool IsExceedingBoxSize(double value)
        {
            const double minBoxDimension = 0.0001;
            const double maxBoxDimension = 10.0;
            if (value <= minBoxDimension || value > maxBoxDimension)
                return true;

            return false;
        }

        public bool Equals(Pudelko other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            double[] pudelkoJeden = {Dlugosc, Szerokosc, Wysokosc};
            double[] pudelkoDwa = {other.Dlugosc, other.Szerokosc, other.Wysokosc};
            Array.Sort(pudelkoJeden);
            Array.Sort(pudelkoDwa);
            for (var i = 0; i < pudelkoJeden.Length; i++)
            {
                if (Math.Abs(pudelkoJeden[i] - pudelkoDwa[i]) > 0.001)
                    return false;
            }

            return true;
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

        public override int GetHashCode()
        {
            return (_dlugosc, _szerokosc, _wysokosc, _jednostkaMiary).GetHashCode();
        }

        public static bool operator ==(Pudelko p1, Pudelko p2) => Equals(p1, p2);
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);
        public static implicit operator double[](Pudelko p) => new double[] {p.Dlugosc, p.Szerokosc, p.Wysokosc};
        public static explicit operator Pudelko(ValueTuple<double, double, double> tuple) => new Pudelko(tuple.Item1, tuple.Item2, tuple.Item3, UnitOfMeasure.Millimeter);
        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            double[] firstBox = p1;
            double[] secondBox = p2;
            double[] forBoth = new double[3];

            Array.Sort(firstBox);
            Array.Sort(secondBox);

            forBoth[0] = firstBox[0] + secondBox[0];
            forBoth[1] = secondBox[1] > firstBox[1] ? secondBox[1] : firstBox[1];
            forBoth[2] = secondBox[2] > firstBox[2] ? secondBox[2] : firstBox[2];

            return new Pudelko(forBoth[0], forBoth[1], forBoth[2]);
        }


        public double this[int i] => i switch
        {
            0 => Dlugosc,
            1 => Szerokosc,
            2 => Wysokosc,
            _ => throw new ArgumentOutOfRangeException()
        };

        public IEnumerator<double> GetEnumerator()
        {
            yield return Dlugosc;
            yield return Szerokosc;
            yield return Wysokosc;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
                    dlugosc = Dlugosc;
                    szerokosc = Szerokosc;
                    wysokosc = Wysokosc;
                    break;
                case "cm":
                    unitFormat = "##0.0";
                    dlugosc = UnitConverter.ToCentimeter(_dlugosc);
                    szerokosc = UnitConverter.ToCentimeter(_szerokosc);
                    wysokosc = UnitConverter.ToCentimeter(_wysokosc);
                    break;
                case "mm":
                    unitFormat = "####";
                    dlugosc = UnitConverter.ToMillimeter(_dlugosc);
                    szerokosc = UnitConverter.ToMillimeter(_szerokosc);
                    wysokosc = UnitConverter.ToMillimeter(_wysokosc);
                    break;
                default:
                    throw new FormatException();
            }

            return $"{dlugosc.ToString(unitFormat, provider)} {format} × {szerokosc.ToString(unitFormat, provider)}" +
                   $" {format} × {wysokosc.ToString(unitFormat, provider)} {format}";
        }

        public static Pudelko Parse(string input)
        {
            var parts = input.Split(' ');
            try
            {
                double[] dimensions = {double.Parse(parts[0]), double.Parse(parts[3]), double.Parse(parts[6])};
                string[] units = {parts[1], parts[4], parts[7]};
                var unitOfMeasure = GetUnitOfMeasure(units);
                return new Pudelko(dimensions[0], dimensions[1], dimensions[2], unitOfMeasure);
            }
            catch
            {
                throw new FormatException();
            }
        }

        private static UnitOfMeasure GetUnitOfMeasure(string[] units)
        {
            if (units.Length != 0)
            {
                var pickedUnit = units[0];
                foreach (var unit in units)
                {
                    if (pickedUnit != unit)
                        throw new FormatException("Wrong format");
                }
            }

            var unitOfMeasure = units[0];
            switch(unitOfMeasure)
            {
                case "m":
                    return UnitOfMeasure.Meter;
                case "cm":
                    return UnitOfMeasure.Centimeter;
                case "mm":
                    return UnitOfMeasure.Millimeter;
                default:
                    throw new FormatException("Wrong format");
            };
        }
    }
}
