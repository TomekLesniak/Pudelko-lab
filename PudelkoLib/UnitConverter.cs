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
    }
}