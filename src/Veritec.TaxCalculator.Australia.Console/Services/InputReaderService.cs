using Veritec.TaxCalculator.Australia.Console.Contracts;
using Veritec.TaxCalculator.Core.Constants;
using Veritec.TaxCalculator.Core.Enums;
using Veritec.TaxCalculator.Core.Model;

namespace Veritec.TaxCalculator.Australia.Console.Services
{
    public class InputReaderService : IInputReaderService
    {
        public Payment GetPaymentInput()
        {
            while (true)
            {
                System.Console.Write("Enter your pay frequency (W for weekly, F for fortnightly, M for monthly): ");

                string? paymentString = System.Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(paymentString))
                {
                    System.Console.WriteLine("Please enter a valid pay frequency.");
                }
                else
                {
                    switch (paymentString.ToUpper())
                    {
                        case PaymentConstant.Weekly:
                            return new Payment(PaymentFrequency.Weekly, "week");
                        case PaymentConstant.Fortnightly:
                            return new Payment(PaymentFrequency.Fortnightly, "fortnight");
                        case PaymentConstant.Monthly:
                            return new Payment(PaymentFrequency.Monthly, "month");
                        default:
                            System.Console.WriteLine("Invalid pay frequency entered. Please try again.");
                            break;
                    }
                }
            }
        }

        public double ReadSalaryPackage()
        {
            while (true)
            {
                System.Console.Write("Enter your salary package amount: ");

                var grossPackageString = System.Console.ReadLine();
                if (!double.TryParse(grossPackageString, out var salaryPackage) || salaryPackage <= 0)
                {
                    System.Console.WriteLine("An invalid amount has been entered.");
                }
                else
                {
                    return salaryPackage;
                }
            }
        }
    }
}
