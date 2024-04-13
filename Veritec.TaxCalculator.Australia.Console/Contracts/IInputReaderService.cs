
using Veritec.TaxCalculator.Core.Model;

namespace Veritec.TaxCalculator.Australia.Console.Contracts
{
    public interface IInputReaderService
    {
        double ReadSalaryPackage();
        Payment GetPaymentInput();
    }
}