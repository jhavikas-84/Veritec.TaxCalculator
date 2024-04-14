
using Veritec.TaxCalculator.Core.Exceptions;

namespace Veritec.TaxCalculator.Australia.Exceptions
{
    public class MedicareLevyException : TaxCalculatorExceptionBase
    {
        public MedicareLevyException()
        {
        }

        public MedicareLevyException(string message)
            : base(message)
        {
        }

        public MedicareLevyException(string message, Exception? inner)
            : base(message, inner)
        {
        }
    }
}
