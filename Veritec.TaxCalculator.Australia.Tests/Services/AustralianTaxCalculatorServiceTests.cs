using FluentAssertions;
using NSubstitute;
using Veritec.TaxCalculator.Australia.Options;
using Veritec.TaxCalculator.Australia.Services;
using Veritec.TaxCalculator.Core.Contracts;
using Veritec.TaxCalculator.Core.Enums;
using Veritec.TaxCalculator.Core.Model;

namespace Veritec.TaxCalculator.Australia.Tests.Services
{
    public class AustralianTaxCalculatorServiceTests
    {
        [Fact]
        public void GetSalaryDetails_ReturnsCorrectSalaryDetails_ForZeroIncome()
        {
            // Arrange
            var annualGrossAmount = 0;
            var payment = new Payment(PaymentFrequency.Monthly, "month");
            var medicareLevyService = Substitute.For<IDeduction>();
            var budgetRepairLevyService = Substitute.For<IDeduction>();
            var incomeTaxService = Substitute.For<IncomeTaxService>(annualGrossAmount, new SuperAnnuationOptions());
            var superannuationService = Substitute.For<IDeduction>();

            medicareLevyService.CalculateDeduction(Arg.Any<double>()).Returns(0); // Mock zero medicare levy
            budgetRepairLevyService.CalculateDeduction(Arg.Any<double>()).Returns(0); // Mock zero budget repair levy
            incomeTaxService.CalculateDeduction(Arg.Any<double>()).Returns(0); // Mock zero income tax
            superannuationService.CalculateDeduction(Arg.Any<double>()).Returns(0); // Mock zero superannuation contribution

            var taxCalculatorService = new AustralianTaxCalculatorService(
                incomeTaxService,
                medicareLevyService,
                budgetRepairLevyService,
                superannuationService);

            // Act
            var result = taxCalculatorService.GetSalaryDetails(annualGrossAmount, payment);

            // Assert
            result.Should().NotBeNull();
            result.GrossPackage.Should().Be(annualGrossAmount);
            result.TaxableIncome.Should().Be(0); // Taxable income should be zero
            result.MedicareLevy.Should().Be(0);
            result.BudgetRepairLevy.Should().Be(0);
            result.IncomeTax.Should().Be(0);
            result.SuperannuationContribution.Should().Be(0);
            result.PaymentFrequency.Should().Be(payment.PaymentDescription);
            result.NetIncome.Should().Be(0); // Net income should be zero
            result.PayPacket.Should().Be(0); // Pay packet should be zero
        }

        [Fact]
        public void GetSalaryDetails_ReturnsCorrectSalaryDetails_ForHighIncome()
        {
            // Arrange
            var superAnnuationOptions = new SuperAnnuationOptions { SuperContributionRate = 9.5 };
            var annualGrossAmount = 250000;
            var annualTaxableIncome = 228310.5;
            var payment = new Payment(PaymentFrequency.Monthly, "month");
            var medicareLevyService = Substitute.For<IDeduction>();
            var budgetRepairLevyService = Substitute.For<IDeduction>();
            var incomeTaxService = Substitute.For<IncomeTaxService>(annualGrossAmount, superAnnuationOptions);
            var superannuationService = Substitute.For<IDeduction>();

            incomeTaxService.AnnualTaxableIncome.Returns(annualTaxableIncome);
            medicareLevyService.CalculateDeduction(Arg.Any<double>()).Returns(4567);
            budgetRepairLevyService.CalculateDeduction(Arg.Any<double>()).Returns(966.20);
            incomeTaxService.CalculateDeduction(Arg.Any<double>()).Returns(76938);
            superannuationService.CalculateDeduction(Arg.Any<double>()).Returns(21689.49);

            var taxCalculatorService = new AustralianTaxCalculatorService(
                incomeTaxService,
                medicareLevyService,
                budgetRepairLevyService,
                superannuationService);

            // Act
            var result = taxCalculatorService.GetSalaryDetails(annualGrossAmount, payment);

            // Assert
            result.Should().NotBeNull();
            result.GrossPackage.Should().Be(annualGrossAmount);
            result.TaxableIncome.Should().Be(annualTaxableIncome);
            result.MedicareLevy.Should().Be(4567);
            result.BudgetRepairLevy.Should().Be(966.20);
            result.IncomeTax.Should().Be(76938);
            result.SuperannuationContribution.Should().Be(21689.49);
            result.PaymentFrequency.Should().Be(payment.PaymentDescription);

            var expectedNetIncome = 145839.31;
            result.NetIncome.Should().Be(expectedNetIncome);

            var expectedPayPacket = expectedNetIncome / 12; // Monthly payment
            result.PayPacket.Should().Be(expectedPayPacket);
        }
    }
}
