using Veritec.TaxCalculator.Core.Enums;

namespace Veritec.TaxCalculator.Core.Model
{
    public record Payment(PaymentFrequency PaymentFrequency, string PaymentDescription);
}
