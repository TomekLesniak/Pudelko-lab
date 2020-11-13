﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
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
            if (value < minBoxDimension || value > maxBoxDimension)
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
            }

            return $"{dlugosc.ToString(unitFormat, provider)} {format} × {szerokosc.ToString(unitFormat, provider)}" +
                   $" {format} × {wysokosc.ToString(unitFormat, provider)} {format}";
        }
    }
}
