
using Veritec.TaxCalculator.Core.Exceptions;

namespace Veritec.TaxCalculator.Australia.Exceptions
{
    public class IncomeTaxException : TaxCalculatorExceptionBase
    {
        public IncomeTaxException()
        {
        }

        public IncomeTaxException(string message)
            : base(message)
        {
        }

        public IncomeTaxException(string message, Exception? inner)
            : base(message, inner)
        {
        }
    }
}
