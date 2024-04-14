using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Veritec.TaxCalculator.Core.Contracts;

namespace Veritec.TaxCalculator.Australia.Tests.Services
{
    public class BudgetRepairLevyServiceTests
    {
        [Fact]
        public void CalculateDeduction_ReturnsZero_ForZeroIncome()
        {
            // Arrange
            var annualTaxableIncome = 0; // Zero income
            var budgetRepairLevyService = Substitute.For<IDeduction>();
            budgetRepairLevyService.CalculateDeduction(Arg.Any<double>()).Returns(annualTaxableIncome);

            // Act
            var result = budgetRepairLevyService.CalculateDeduction(annualTaxableIncome);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void CalculateDeduction_ReturnsZero_ForIncomeEqualToThreshold()
        {
            // Arrange
            var annualTaxableIncome = 180000; // Income equal to threshold
            var budgetRepairLevyService = Substitute.For<IDeduction>();
            budgetRepairLevyService.CalculateDeduction(Arg.Any<double>()).Returns(0);

            // Act
            var result = budgetRepairLevyService.CalculateDeduction(annualTaxableIncome);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void CalculateDeduction_ReturnsCorrectDeduction_ForHighIncomeWithMultipleThresholds()
        {
            // Arrange
            var fixture = new Fixture();
            var annualTaxableIncome = fixture.Create<double>() % (200000 - 180000) + 180001; // Any income greater than 180000
            var expectedDeduction = (annualTaxableIncome - 180000) * 0.02;
            var budgetRepairLevyService = Substitute.For<IDeduction>();
            budgetRepairLevyService.CalculateDeduction(Arg.Any<double>()).Returns(expectedDeduction);

            // Act
            var result = budgetRepairLevyService.CalculateDeduction(annualTaxableIncome);

            // Assert
            result.Should().Be(expectedDeduction);
        }

        [Fact]
        public void CalculateDeduction_ThrowsException_ForNegativeIncome()
        {
            // Arrange
            var annualTaxableIncome = -1000; // Negative income
            var budgetRepairLevyService = Substitute.For<IDeduction>();
            budgetRepairLevyService.CalculateDeduction(Arg.Any<double>()).Returns(x => { throw new ArgumentOutOfRangeException(); });

            // Act & Assert
            budgetRepairLevyService.Invoking(x => x.CalculateDeduction(annualTaxableIncome))
                .Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
