using Veritec.TaxCalculator.Australia.Exceptions;
using Veritec.TaxCalculator.Core.Extensions;
using Veritec.TaxCalculator.Core.Services;

namespace Veritec.TaxCalculator.Australia.Services
{
    public class BudgetRepairLevyService() : TaxDeductionServiceBase
    {
        public override double CalculateDeduction(double annualTaxableIncome)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(annualTaxableIncome));
            ArgumentOutOfRangeException.ThrowIfLessThan(annualTaxableIncome, 0);

            try
            {
                var roundedTaxableIncome = annualTaxableIncome.RoundToNearestDollar(MidpointRounding.ToNegativeInfinity);

                switch (roundedTaxableIncome)
                {
                    case <= 180000:
                        return 0;
                    default:
                        var excessTaxableIncome = roundedTaxableIncome - 180000;
                        var budgetRepairLevy = excessTaxableIncome * 0.02D;
                        return budgetRepairLevy;
                }
            }
            catch (Exception ex)
            {
                throw new BudgetRepairLevyException(ex.Message, ex.InnerException);
            }
        }
    }
}
