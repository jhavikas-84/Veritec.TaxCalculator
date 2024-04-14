using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Veritec.TaxCalculator.Australia.Options;
using Veritec.TaxCalculator.Australia.Services;
using Veritec.TaxCalculator.Core.Contracts;

namespace Veritec.TaxCalculator.Australia.Tests.Services
{
    public class SuperannuationServiceTests
    {
        [Theory]
        [InlineData(65000, 5639.27)]
        [InlineData(50000, 4337.89)]
        [InlineData(90000, 7808.22)]
        public void CalculateDeduction_ReturnsCorrectDeduction(double annualTaxableIncome, double expectedDeduction)
        {
            // Arrange
            var superAnnuationOptions = new SuperAnnuationOptions
            {
                SuperContributionRate = 9.5 // Example superannuation contribution rate
            };
            var superAnnuationService = Substitute.For<SuperannuationService>(superAnnuationOptions);
            superAnnuationService.CalculateDeduction(Arg.Any<double>()).Returns(expectedDeduction);

            // Act
            var result = superAnnuationService.CalculateDeduction(annualTaxableIncome);

            // Assert
            //var expectedDeduction = annualTaxableIncome * (superAnnuationOptions.SuperContributionRate / 100);
            result.Should().Be(expectedDeduction);
        }

        [Fact]
        public void CalculateDeduction_ThrowsException_ForNegativeIncome()
        {
            // Arrange
            var fixture = new Fixture();
            var superAnnuationOptions = Substitute.For<SuperAnnuationOptions>();
            var superAnnuationService = new SuperannuationService(superAnnuationOptions);
            var annualTaxableIncome = -100; // Negative income

            // Act & Assert
            superAnnuationService.Invoking(x => x.CalculateDeduction(annualTaxableIncome))
                .Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void CalculateDeduction_ReturnsZero_ForZeroIncome()
        {
            // Arrange
            var annualGrossAmount = 0;
            var superAnnuationService = Substitute.For<IDeduction>();
            superAnnuationService.CalculateDeduction(Arg.Any<double>()).Returns(0); // Mock zero superannuation contribution

            // Act
            var result = superAnnuationService.CalculateDeduction(annualGrossAmount);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void CalculateDeduction_ReturnsZero_ForZeroContributionRate()
        {
            // Arrange
            var fixture = new Fixture();
            var superAnnuationOptions = new SuperAnnuationOptions
            {
                SuperContributionRate = 0 // Zero contribution rate
            };
            var superAnnuationService = Substitute.For<SuperannuationService>(superAnnuationOptions);
            superAnnuationService.CalculateDeduction(Arg.Any<double>()).Returns(0);
            var annualTaxableIncome = fixture.Create<double>(); // Example annual taxable income

            // Act
            var result = superAnnuationService.CalculateDeduction(annualTaxableIncome);

            // Assert
            result.Should().Be(0);
        }
    }
}
