// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Veritec.TaxCalculator.Australia.Console.Contracts;
using Veritec.TaxCalculator.Australia.Console.DTO;
using Veritec.TaxCalculator.Australia.Console.Services;
using Veritec.TaxCalculator.Australia.Options;
using Veritec.TaxCalculator.Australia.Services;
using Veritec.TaxCalculator.Core.Contracts;
using Veritec.TaxCalculator.Core.Enums;

ServiceProvider serviceProvider = new ServiceCollection()
    .AddLogging((loggingBuilder) => loggingBuilder
        .SetMinimumLevel(LogLevel.Trace)
        .AddConsole()
        )
    //.AddSingleton<IDeduction, TaxDeductionServiceBase>()
    //.AddSingleton<IInputReaderService, InputReaderService>()
    //.AddSingleton<IDisplayService, DisplayService>()
    .BuildServiceProvider();

var logger = serviceProvider!.GetService<ILoggerFactory>()!.CreateLogger<Program>();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var options =
    configuration.GetSection(nameof(SuperAnnuationOptions))
        .Get<IOptions<SuperAnnuationOptions>>()!;

IDeduction deduction;
IInputReaderService inputReaderService = new InputReaderService();

var annualGrossAmount = inputReaderService.ReadSalaryPackage();
var paymentFrequency = inputReaderService.GetPaymentInput();

var incomeTax = new IncomeTaxService(annualGrossAmount, options);
var annualTaxableIncome = incomeTax.AnnualTaxableIncome;
var taxAmount = incomeTax.CalculateDeduction(annualTaxableIncome);

deduction = new MedicareLevyService();
var medicareLevy = deduction.CalculateDeduction(annualTaxableIncome);

deduction = new BudgetRepairLevyService();
var budgetRepairLevy = deduction.CalculateDeduction(annualTaxableIncome);

deduction = new SuperannuationService(options);
var superannuationContribution = deduction.CalculateDeduction(annualGrossAmount);

var totalDeduction = medicareLevy + budgetRepairLevy + taxAmount;
var netIncome = annualGrossAmount - superannuationContribution - totalDeduction;

IDisplayService displayService = new DisplayService();
displayService
    .ShowResults(
        new SalaryDetails(
                GrossPackage: annualGrossAmount,
                TaxableIncome: annualTaxableIncome,
                BudgetRepairLevy: budgetRepairLevy,
                IncomeTax: taxAmount,
                MedicareLevy: medicareLevy,
                NetIncome: netIncome,
                PayPacket: GetPayPacket(),
                PaymentFrequency: paymentFrequency.PaymentDescription,
                SuperannuationContribution: superannuationContribution
        ));

double GetPayPacket()
{
    return paymentFrequency.PaymentFrequency switch
    {
        PaymentFrequency.Weekly => netIncome / (int)PaymentFrequency.Weekly,
        PaymentFrequency.Fortnightly => netIncome / (int)PaymentFrequency.Fortnightly,
        PaymentFrequency.Monthly => netIncome / (int)PaymentFrequency.Monthly,
        _ => throw new ArgumentException(nameof(paymentFrequency.PaymentFrequency)),
    };
};
