
namespace Veritec.TaxCalculator.Core.Contracts
{
    public interface IDeduction
    {
        double CalculateDeduction(double annualTaxableIncome);
    }
}
