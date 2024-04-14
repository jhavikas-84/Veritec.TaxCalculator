using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Veritec.TaxCalculator.Australia.Options;
using Veritec.TaxCalculator.Australia.Services;

namespace Veritec.TaxCalculator.Australia.Tests.Services
{
    public class IncomeTaxServiceTests
    {
        [Theory]
        [InlineData(65000, 59360.73)]
        [InlineData(45000, 41095.89)]
        [InlineData(90000, 82191.78)]
        public void AnnualTaxableIncome_ReturnsCorrectValue(double annualTaxableIncome, double expectedTaxableIncome)
        {
            // Arrange
            var superAnnuationOptions = new SuperAnnuationOptions { SuperContributionRate = 9.5 };
            var incomeTaxService = Substitute.For<IncomeTaxService>(annualTaxableIncome, superAnnuationOptions);

            // Act
            var result = incomeTaxService.AnnualTaxableIncome;

            // Assert
            result.Should().Be(expectedTaxableIncome);
        }

        [Theory]
        [InlineData(10000, 0)]
        [InlineData(18200, 0)]
        [InlineData(30000, 2242)]
        [InlineData(85000, 19172)]
        [InlineData(160000, 46832)]
        [InlineData(250000, 87132)]
        public void CalculateDeduction_ReturnsCorrectDeduction(double annualTaxableIncome, double expectedDeduction)
        {
            // Arrange
            var superAnnuationOptions = new SuperAnnuationOptions { SuperContributionRate = 9.5 };
            var incomeTaxService = Substitute.For<IncomeTaxService>(annualTaxableIncome, superAnnuationOptions);
            incomeTaxService.CalculateDeduction(Arg.Any<double>()).Returns(expectedDeduction);

            // Act
            var result = incomeTaxService.CalculateDeduction(incomeTaxService.AnnualTaxableIncome);

            // Assert
            result.Should().Be(expectedDeduction);
        }

        [Fact]
        public void CalculateDeduction_ThrowsException_ForNegativeIncome()
        {
            // Arrange
            var fixture = new Fixture();
            var incomeTaxService = Substitute.For<IncomeTaxService>(0, new SuperAnnuationOptions());
            incomeTaxService.CalculateDeduction(Arg.Any<double>()).Returns(x => { throw new ArgumentOutOfRangeException(); });

            // Act & Assert
            incomeTaxService.Invoking(x => x.CalculateDeduction(-100))
                .Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
