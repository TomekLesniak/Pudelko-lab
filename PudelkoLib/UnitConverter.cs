namespace PudelkoLib
{
    public static class UnitConverter
    {
        public static double ToMeter(double value, UnitOfMeasure currentUnitMeasure)
        {
            if (currentUnitMeasure == UnitOfMeasure.Millimeter)
            {
                value /= 10.0;
                currentUnitMeasure = UnitOfMeasure.Centimeter;
            }
            if (currentUnitMeasure == UnitOfMeasure.Centimeter)
            {
                value /= 100.0;
            }

            return value;
        }

        public static double ToCentimeter(double value) => value * 100;
        public static double ToMillimeter(double value) => value * 1000;
    }
}