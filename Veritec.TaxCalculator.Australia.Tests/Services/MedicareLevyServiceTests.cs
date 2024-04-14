using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Veritec.TaxCalculator.Australia.Options;
using Veritec.TaxCalculator.Australia.Services;
using Veritec.TaxCalculator.Core.Contracts;

namespace Veritec.TaxCalculator.Australia.Tests.Services
{
    public class MedicareLevyServiceTests
    {
        [Theory]
        [InlineData(65000, 1188)]
        [InlineData(90000, 1644)]
        [InlineData(45000, 822)]
        public void CalculateDeduction_ReturnsZero_ForLowIncome(double annualTaxableIncome, double expectedDeduction)
        {
            // Arrange
            var superAnnuationOptions = new SuperAnnuationOptions
            {
                SuperContributionRate = 9.5 // Example superannuation contribution rate
            };
            var medicareLevyService = Substitute.For<IDeduction>();
            medicareLevyService.CalculateDeduction(Arg.Any<double>()).Returns(expectedDeduction);
            var incomeTaxService = Substitute.For<IncomeTaxService>(annualTaxableIncome,superAnnuationOptions);

            // Act
            var result = medicareLevyService.CalculateDeduction(incomeTaxService.AnnualTaxableIncome);

            // Assert
            result.Should().Be(expectedDeduction);
        }
       
        [Fact]
        public void CalculateDeduction_ThrowsException_ForNegativeIncome()
        {
            // Arrange
            var fixture = new Fixture();
            var medicareLevyService = Substitute.For<IDeduction>();
            medicareLevyService.CalculateDeduction(Arg.Any<double>()).Returns( x => { throw new ArgumentOutOfRangeException(); });
            var annualTaxableIncome = -fixture.Create<double>(); // Negative income

            // Act & Assert
            medicareLevyService.Invoking(x => x.CalculateDeduction(annualTaxableIncome))
                .Should().Throw<ArgumentOutOfRangeException>();
        }         
    }

}
