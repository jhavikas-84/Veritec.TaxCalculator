
namespace Veritec.TaxCalculator.Core.Extensions
{
    public static class DoubleExtension
    {
        public static double RoundToNearestDollar(this double value, MidpointRounding midpointRounding)
        {
            return Math.Round(value, 0, midpointRounding);
        }

        public static double RoundToDollarAndCents(this double value, MidpointRounding midpointRounding)
        {
            return Math.Round(value, 2, midpointRounding);
        }

        public static string ToCurrency(this double amount, string message)
        {
            return $"{message}: {amount:C}";
        }
    }
}
