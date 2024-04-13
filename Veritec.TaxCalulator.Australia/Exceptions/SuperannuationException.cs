
using Veritec.TaxCalculator.Core.Exceptions;

namespace Veritec.TaxCalculator.Australia.Exceptions
{
    public class SuperannuationException : TaxCalculatorExceptionBase
    {
        public SuperannuationException()
        {
        }

        public SuperannuationException(string message)
            : base(message)
        {
        }

        public SuperannuationException(string message, Exception? inner)
            : base(message, inner)
        {
        }
    }
}
