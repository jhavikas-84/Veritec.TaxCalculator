using Veritec.TaxCalculator.Australia.Exceptions;
using Veritec.TaxCalculator.Core.Extensions;
using Veritec.TaxCalculator.Core.Services;

namespace Veritec.TaxCalculator.Australia.Services
{
    public class MedicareLevyService() : TaxDeductionServiceBase
    {
        public override double CalculateDeduction(double annualTaxableIncome)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(annualTaxableIncome));
            ArgumentOutOfRangeException.ThrowIfLessThan(annualTaxableIncome, 0);

            try
            {
                var roundedTaxableIncome = annualTaxableIncome.RoundToNearestDollar(MidpointRounding.ToPositiveInfinity);

                switch (roundedTaxableIncome)
                {
                    case <= 21335:
                        return 0;
                    case <= 26668:
                        {
                            var excessIncome = roundedTaxableIncome - 21335;
                            var unroundedMedicareLevy = excessIncome * 0.1D;
                            return RoundedUpAmount(unroundedMedicareLevy);
                        }
                    default:
                        {
                            var unroundedMedicareLevy = roundedTaxableIncome * 0.02D;
                            return RoundedUpAmount(unroundedMedicareLevy); ;
                        }
                }
            }
            catch (Exception ex)
            {
                throw new MedicareLevyException(ex.Message, ex.InnerException);
            }
        }
    }
}