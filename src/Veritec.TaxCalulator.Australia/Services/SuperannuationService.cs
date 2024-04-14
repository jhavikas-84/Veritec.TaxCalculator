using Microsoft.Extensions.Options;
using Veritec.TaxCalculator.Australia.Exceptions;
using Veritec.TaxCalculator.Australia.Options;
using Veritec.TaxCalculator.Core.Services;

namespace Veritec.TaxCalculator.Australia.Services
{
    public class SuperannuationService(SuperAnnuationOptions superAnnuation) : TaxDeductionServiceBase
    {
        public override double CalculateDeduction(double annualTaxableIncome)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(annualTaxableIncome));
            ArgumentOutOfRangeException.ThrowIfLessThan(annualTaxableIncome, 0);

            try
            {
                var taxableIncome = RoundedAmountWithCents(annualTaxableIncome / (1 + superAnnuation.SuperContributionRate * 0.01D));
                var unroundedSuperannuation = annualTaxableIncome - taxableIncome;
                return RoundedAmountWithCents(unroundedSuperannuation);
            }
            catch (Exception ex)
            {
                throw new SuperannuationException(ex.Message, ex.InnerException);
            }
        }
    }
}
