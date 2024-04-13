
namespace Veritec.TaxCalculator.Australia.Console.DTO
{
    public record SalaryDetails
    (
        double GrossPackage,
        double SuperannuationContribution,
        double TaxableIncome,
        double MedicareLevy,
        double BudgetRepairLevy,
        double IncomeTax,
        double NetIncome,
        double PayPacket,
        string PaymentFrequency
    );
}
