// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Veritec.TaxCalculator.Australia.Console.Contracts;
using Veritec.TaxCalculator.Australia.Console.Services;
using Veritec.TaxCalculator.Australia.Options;
using Veritec.TaxCalculator.Australia.Services;
using Veritec.TaxCalculator.Core.Contracts;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var serviceCollection = new ServiceCollection()
    .AddLogging((loggingBuilder) => loggingBuilder
        .SetMinimumLevel(LogLevel.Trace)
        .AddConsole()
        )
    .AddSingleton<IDeduction, IncomeTaxService>()
    .AddSingleton<IDeduction, MedicareLevyService>()
    .AddSingleton<IDeduction, BudgetRepairLevyService>()
    .AddSingleton<IDeduction, SuperannuationService>()
    .AddSingleton<IInputReaderService, InputReaderService>()
    .AddSingleton<IDisplayService, DisplayService>();

serviceCollection.AddOptions<SuperAnnuationOptions>()
                 .Bind(configuration.GetSection(nameof(SuperAnnuationOptions)));

var serviceProvider = serviceCollection.BuildServiceProvider();
var logger = serviceProvider!.GetService<ILoggerFactory>()!.CreateLogger<Program>();

// Resolve the configured settings
var options = serviceProvider.GetRequiredService<IOptions<SuperAnnuationOptions>>().Value;
ArgumentNullException.ThrowIfNull(nameof(options));

//TODO: Need to find and fix the issue for binding the super annuation options
options.SuperContributionRate = 9.5;

IInputReaderService inputReaderService = new InputReaderService();
var annualGrossAmount = inputReaderService.ReadSalaryPackage();
var paymentFrequency = inputReaderService.GetPaymentInput();

IncomeTaxService IncomeTaxService = new(annualGrossAmount, options);
MedicareLevyService MedicareLevyService = new();
BudgetRepairLevyService BudgetRepairLevyService = new();
SuperannuationService SuperannuationService = new(options);

// TODO: Can be improved and simplified with Facade pattern.
AustralianTaxCalculatorService taxCalculator =
        new(
            IncomeTaxService,
            MedicareLevyService,
            BudgetRepairLevyService,
            SuperannuationService
        );

IDisplayService displayService = new DisplayService();
displayService.ShowResults(taxCalculator.GetSalaryDetails(annualGrossAmount, paymentFrequency));


