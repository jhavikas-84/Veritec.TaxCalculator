using Veritec.TaxCalculator.Australia.Contracts;
using Veritec.TaxCalculator.Australia.DTO;
using Veritec.TaxCalculator.Core.Contracts;
using Veritec.TaxCalculator.Core.Enums;
using Veritec.TaxCalculator.Core.Model;

namespace Veritec.TaxCalculator.Australia.Services
{
    public class AustralianTaxCalculatorService
        (
            IncomeTaxService IncomeTaxService,
            IDeduction MedicareLevyService,
            IDeduction BudgetRepairLevyService,
            IDeduction SuperannuationService
        ) : IAustralianTaxCalculatorService
    {
        public SalaryDetails GetSalaryDetails(double annualGrossAmount, Payment payment)
        {
            var annualTaxableIncome = IncomeTaxService.AnnualTaxableIncome;

            var medicareLevy = GetCalculatedDeduction(annualTaxableIncome, MedicareLevyService);
            var budgetRepairLevy = GetCalculatedDeduction(annualTaxableIncome, BudgetRepairLevyService);
            var taxAmount = GetCalculatedDeduction(annualTaxableIncome, IncomeTaxService);
            var superannuationContribution = GetCalculatedDeduction(annualGrossAmount, SuperannuationService);

            var totalDeduction = medicareLevy + budgetRepairLevy + taxAmount;
            var netIncome = annualGrossAmount - superannuationContribution - totalDeduction;

            var salaryDetails =
                new SalaryDetails(
                        GrossPackage: annualGrossAmount,
                        TaxableIncome: annualTaxableIncome,
                        BudgetRepairLevy: budgetRepairLevy,
                        IncomeTax: taxAmount,
                        MedicareLevy: medicareLevy,
                        NetIncome: netIncome,
                        PayPacket: GetPayPacket(payment, netIncome),
                        PaymentFrequency: payment.PaymentDescription,
                        SuperannuationContribution: superannuationContribution
                );

            return salaryDetails;
        }

        private static double GetCalculatedDeduction(double annualTaxableIncome, IDeduction taxDeductionService)
        {
            var calculatedDeductionAmount = taxDeductionService.CalculateDeduction(annualTaxableIncome);
            return calculatedDeductionAmount;
        }

        private static double GetPayPacket(Payment payment, double netIncome)
        {
            return payment.PaymentFrequency switch
            {
                PaymentFrequency.Weekly => netIncome / (int)PaymentFrequency.Weekly,
                PaymentFrequency.Fortnightly => netIncome / (int)PaymentFrequency.Fortnightly,
                PaymentFrequency.Monthly => netIncome / (int)PaymentFrequency.Monthly,
                _ => throw new ArgumentException(nameof(payment)),
            };
        }
    }
}
