using Veritec.TaxCalculator.Australia.DTO;
using Veritec.TaxCalculator.Core.Model;

namespace Veritec.TaxCalculator.Australia.Contracts
{
    public interface IAustralianTaxCalculatorService
    {
        SalaryDetails GetSalaryDetails(double annualGrossAmount, Payment payment);
    }
}
