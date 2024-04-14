
using Veritec.TaxCalculator.Core.Exceptions;

namespace Veritec.TaxCalculator.Australia.Exceptions
{
    public class BudgetRepairLevyException : TaxCalculatorExceptionBase
    {
        public BudgetRepairLevyException()
        {
        }

        public BudgetRepairLevyException(string message)
            : base(message)
        {
        }

        public BudgetRepairLevyException(string message, Exception? inner)
            : base(message, inner)
        {
        }
    }
}
