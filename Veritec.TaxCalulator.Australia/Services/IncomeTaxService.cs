using Microsoft.Extensions.Options;
using Veritec.TaxCalculator.Australia.Exceptions;
using Veritec.TaxCalculator.Australia.Options;
using Veritec.TaxCalculator.Core.Extensions;
using Veritec.TaxCalculator.Core.Services;

namespace Veritec.TaxCalculator.Australia.Services
{
    public class IncomeTaxService(double annualTaxableIncome, SuperAnnuationOptions superAnnuation) : TaxDeductionServiceBase
    {
        public double AnnualTaxableIncome
        {
            get
            {
                try
                {
                    var unroundedTaxableIncome = annualTaxableIncome / (1 + superAnnuation.SuperContributionRate * 0.01D);
                    return unroundedTaxableIncome.RoundToDollarAndCents(MidpointRounding.ToNegativeInfinity);
                }
                catch (Exception ex)
                {
                    throw new IncomeTaxException(ex.Message, ex.InnerException);
                }

            }
        }

        public override double CalculateDeduction(double annualTaxableIncome)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(annualTaxableIncome));
            ArgumentOutOfRangeException.ThrowIfLessThan(annualTaxableIncome, 0);

            try
            {
                var roundedTaxableIncome = annualTaxableIncome.RoundToNearestDollar(MidpointRounding.ToNegativeInfinity);

                double excess;
                switch (roundedTaxableIncome)
                {
                    case <= 18200:
                        return 0;
                    case <= 37000:
                        excess = GetExcessAmount(roundedTaxableIncome, 19, 18200);
                        break;
                    case <= 87000:
                        excess = 3572 + GetExcessAmount(roundedTaxableIncome, 32.5D, 37000);
                        break;
                    case <= 180000:
                        excess = 19822 + GetExcessAmount(roundedTaxableIncome, 37, 87000);
                        break;
                    default:
                        excess = 54232 + GetExcessAmount(roundedTaxableIncome, 47, 180000);
                        break;
                }

                return excess.RoundToNearestDollar(MidpointRounding.ToPositiveInfinity);

                static double GetExcessAmount(double taxableIncome, double taxPercentage, double excessThreshold)
                {
                    var excessIncome = taxableIncome - excessThreshold;
                    var excess = excessIncome * (taxPercentage * 0.01D);
                    return Math.Round(excess, 0, MidpointRounding.ToPositiveInfinity);
                }
            }
            catch (Exception ex)
            {
                throw new IncomeTaxException(ex.Message, ex.InnerException);
            }
        }
    }
}
