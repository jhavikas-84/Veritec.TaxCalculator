
namespace Veritec.TaxCalculator.Core.Exceptions
{
    public abstract class TaxCalculatorExceptionBase : Exception
    {
        public TaxCalculatorExceptionBase()
        {
        }

        public TaxCalculatorExceptionBase(string message)
            : base(message)
        {
        }

        public TaxCalculatorExceptionBase(string message, Exception? inner)
            : base(message, inner)
        {
        }
    }
}
