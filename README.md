# <h1 align="center" id="title">Veritec.TaxCalulator</h1>

<p id="description">C# Windows console application that allows a user to enter a gross salary package and a pay frequency and then show them a breakdown of this salary and what their pay packet will be.</p>

  


# Getting Started
This is a basic Tax Calculator app developed using .Net Core 8, to install this version please visit https://dotnet.microsoft.com/en-us/download/dotnet/8.0


## 💪 Features
### Veritec.TaxCalculator.Core
* This is the core application that provides the basic infrastructure for tax calculation, The base class `TaxDeductionServiceBase.cs` *supports the SOLID Principles (OCP) pattern.*
* Also keep in mind that we may have a few other clients and their Taxation Rules could be different, so this base class will be quite useful for extending and implementing the Taxation Rules based on the clients.

### Veritec.TaxCalculator.Australia
* This is one of the clients (Australia) where we have Medicare Levy, Superannuation and other tax deduction components.
* Here the services [`BudgetRepairLevyService`, `IncomeTaxService`, `MedicareLevyService` and `SuperannuationService` have been inherited from the `TaxDeductionServiceBase.cs` class.
* `AustralianTaxCalculatorService` is an encapsulated service, it takes the input amount and frequency and returns a DTO object of `SalaryDetails` that hides the complexity from the client project **Veritec.TaxCalculator.Australia.Console**.

### Veritec.TaxCalculator.Australia.Console
* This is the client application that has the **DI Patterns (SOLID Principles)** and validations to user input.
* The services `InputReaderService` and `DisplayService` are used for input and output to the console.


# Unit Test Case Results
All the services have been unit-tested and there are 26 test cases, Below is the screenshot of the test results.

![image](https://github.com/jhavikas-84/Veritec.TaxCalculator/assets/144097762/eda88ede-4bd1-4407-ac80-1bcfbb5e215e)

  
<h2>💻 Built with</h2>

Technologies used in the project:

*   C#.Net
*   .Net Core 8.0
*   SOLID Principles
*   Visual Studio 2022 Preview
*   NSubstitute
*   Auto Fixture
*   Fluent Assertions


# TODO List
* Usage of the `Logging` in the services
* Enhancement in the exception handling to use the `Middleware Pattern`
* Enhancement and the bug fix for the `IOptions` to bind the config values so that we can have the flexibility of decoupling the numbers, for eg: The superannuation rate from 9.5 to 11.
* Usage of `Facade Pattern` for object creations


Thank you for reading 😃
