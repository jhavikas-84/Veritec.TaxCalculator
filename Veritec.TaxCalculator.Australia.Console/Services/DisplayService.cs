using Veritec.TaxCalculator.Australia.Console.Contracts;
using Veritec.TaxCalculator.Australia.Console.DTO;
using Veritec.TaxCalculator.Core.Extensions;

namespace Veritec.TaxCalculator.Australia.Console.Services
{
    public class DisplayService() : IDisplayService
    {
        public void ShowResults(SalaryDetails salaryDetails)
        {
            System.Console.WriteLine(Environment.NewLine);

            System.Console.WriteLine("Calculating salary details...");
            System.Console.WriteLine(Environment.NewLine);

            System.Console.WriteLine(salaryDetails.GrossPackage.ToCurrency("Gross package"));

            System.Console.WriteLine(salaryDetails.SuperannuationContribution.ToCurrency("Superannuation"));
            System.Console.WriteLine(Environment.NewLine);

            System.Console.WriteLine(salaryDetails.TaxableIncome.ToCurrency("Taxable income"));
            System.Console.WriteLine(Environment.NewLine);

            System.Console.WriteLine("Deductions:");
            System.Console.WriteLine(salaryDetails.MedicareLevy.ToCurrency("Medicare Levy"));
            System.Console.WriteLine(salaryDetails.BudgetRepairLevy.ToCurrency("Budget Repair Levy"));
            System.Console.WriteLine(salaryDetails.IncomeTax.ToCurrency("Income Tax"));
            System.Console.WriteLine(Environment.NewLine);

            System.Console.WriteLine(salaryDetails.NetIncome.ToCurrency("Net income"));

            System.Console.WriteLine($"{salaryDetails.PayPacket.ToCurrency("Pay packet")} per {salaryDetails.PaymentFrequency}" );
        }
    }
}
