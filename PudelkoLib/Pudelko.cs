using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace PudelkoLib
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        private readonly double _length;
        private readonly double _width;
        private readonly double _height;
        private readonly UnitOfMeasure _unitOfMeasure;

        public double A => Math.Round(_length, 4);
        public double B => Math.Round(_width, 4);
        public double C => Math.Round(_height, 4);
        public double Volume => Math.Round(A * B * C, 9);
        public double Area =>
            Math.Round((2 * A * B) + (2 * A * C) + (2 * B * C), 6);

        public Pudelko()
        {
            _length = 0.1;
            _width = 0.1;
            _height = 0.1;
            _unitOfMeasure = UnitOfMeasure.Meter;
        }

        public Pudelko(double length, UnitOfMeasure unitOfMeasure = UnitOfMeasure.Meter) : this()
        {
            _length = UnitConverter.ToMeter(length, unitOfMeasure);
            if (IsExceedingBoxSize(_length))
                throw new ArgumentOutOfRangeException();

            _unitOfMeasure = unitOfMeasure;
        }

        public Pudelko(double length, double width, UnitOfMeasure unitOfMeasure = UnitOfMeasure.Meter) : this(length, unitOfMeasure)
        {
            _width = UnitConverter.ToMeter(width, unitOfMeasure);
            if (IsExceedingBoxSize(_width))
                throw new ArgumentOutOfRangeException();
        }

        public Pudelko(double length , double width, double height,
            UnitOfMeasure unitOfMeasure = UnitOfMeasure.Meter) : this(length, width, unitOfMeasure)
        {
            _height = UnitConverter.ToMeter(height, unitOfMeasure);
            if (IsExceedingBoxSize(_height))
                throw new ArgumentOutOfRangeException();
        }

        public bool Equals(Pudelko other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            double[] boxFirst = {A, B, C};
            double[] boxSecond = {other.A, other.B, other.C};
            Array.Sort(boxFirst);
            Array.Sort(boxSecond);
            for (var i = 0; i < boxFirst.Length; i++)
            {
                if (Math.Abs(boxFirst[i] - boxSecond[i]) > 0.001)
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
            return (_length, _width, _height, _unitOfMeasure).GetHashCode();
        }

        public static bool operator ==(Pudelko p1, Pudelko p2) => Equals(p1, p2);
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);
        public static implicit operator double[](Pudelko p) => new double[] {p.A, p.B, p.C};
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
            0 => A,
            1 => B,
            2 => C,
            _ => throw new ArgumentOutOfRangeException()
        };

        public IEnumerator<double> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
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
            var length = 0.0;
            var width = 0.0;
            var height = 0.0;
            switch (format)
            {
                case "m":
                    unitFormat = "0.000";
                    length = A;
                    width = B;
                    height = C;
                    break;
                case "cm":
                    unitFormat = "##0.0";
                    length = UnitConverter.ToCentimeter(_length);
                    width = UnitConverter.ToCentimeter(_width);
                    height = UnitConverter.ToCentimeter(_height);
                    break;
                case "mm":
                    unitFormat = "####";
                    length = UnitConverter.ToMillimeter(_length);
                    width = UnitConverter.ToMillimeter(_width);
                    height = UnitConverter.ToMillimeter(_height);
                    break;
                default:
                    throw new FormatException();
            }

            return $"{length.ToString(unitFormat, provider)} {format} × {width.ToString(unitFormat, provider)}" +
                   $" {format} × {height.ToString(unitFormat, provider)} {format}";
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

        private bool IsExceedingBoxSize(double value)
        {
            const double minBoxDimension = 0.0001;
            const double maxBoxDimension = 10.0;
            return value <= minBoxDimension || value > maxBoxDimension;
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
