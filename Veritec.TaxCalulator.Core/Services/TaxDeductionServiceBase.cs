
using Veritec.TaxCalculator.Core.Contracts;
using Veritec.TaxCalculator.Core.Extensions;

namespace Veritec.TaxCalculator.Core.Services
{
    public abstract class TaxDeductionServiceBase : IDeduction
    {
        public abstract double CalculateDeduction(double annualTaxableIncome);

        public virtual double RoundedUpAmount(double amount)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(amount));
            ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

            return amount.RoundToNearestDollar(MidpointRounding.ToPositiveInfinity);
        }

        public virtual double RoundedDownAmount(double amount)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(amount));
            ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

            return amount.RoundToNearestDollar(MidpointRounding.ToNegativeInfinity);
        }

        public virtual double RoundedAmountWithCents(double amount)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(amount));
            ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

            return amount.RoundToDollarAndCents(MidpointRounding.ToNegativeInfinity);
        }
    }
}
