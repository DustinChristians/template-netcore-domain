# .NET Core Domain Template
A sample .NET Core CRUD solution to be used as a template for new projects.

# Features
- The [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- A Microsoft SQL Server Local Database with seeded data so you can spin up the project and test it out. See the spin up instructions below.
- Entity Framework Core O/RM with code first migrations.
- A base repository that includes all normal CRUD operations.
- Search methods and extensions for easy searching within repositories.
- Support for bulk database operations like BulkCreate, BulkUpdate and BulkDelete thanks to the free [EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions) package.
- A RESTful API that includes an endpoint for messages, users and project settings. The messages and users tables are used for setting up project patterns and can be removed if they aren't needed.
- An Event Log powered by Serilog. Events are logged to the database and the file system as a backup.
- A Hangfire Scheduler project. There is a single scheduled task that exists to keep the Event Log size down to a minimum. More tasks can easily be added with minimal configuration. Task settings are placed in the appsettings.json file.
- NUnit tests for the existing messages, users and settings repositories.

## Localhost Spinup Instructions
- Clone this repository and open it in Visual Studio 2019.
- If you aren't on the latest update for Visual Studio 2019 then you should perform the update to get .NET Core 3.1. In the toolbar, Go to Help -> Check for updates -> Update.
- Set the WebApi project and Scheduler project as the startup projects. Right click the solution and select **properties**. Under **Common Properties** select **Startup Project** then **Multiple startup projects**. Set the **WebApi** project and **Scheduler** project to **Start**.
- The database is a localdb so once the project is run for the first time the local database will be created using Entity Framework code-first migrations and seeded with test data. 
- Upon startup you will see all the seeded **message** data. A second tab will open and display the Hangfire Dashboard. There should be one recurring job for reducing the size of the database every five minutes.

## Projects

Projects are setup to conform to the [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) guidelines.

**CompanyName.ProjectName.WebApi**
A RESTful Web Api project for communication with a UI layer.

**CompanyName.ProjectName.Scheduler**
A project for implementing scheduled tasks with the Hangfire library. New scheduled tasks can be added to this project. Task settings, like cron expressions, are added to this project's appsettings.json file. 

**CompanyName.ProjectName.Core**
A core project to hold domain models, aggregates, interfaces, constants etc.

**CompanyName.ProjectName.Infrastructure**
A project to hold services and business logic.

**CompanyName.ProjectName.Configuration**
Configurations for Dependency Injection, AutoMapper and Entity Framework Core to be shared across projects.

**CompanyName.ProjectName.Repository**
This repository uses Entity Framework Core and the Repository Pattern for CRUD operations.

Running Code First Migrations:
1. Open the **Package Manager Console** window in Visual Studio.
2. Set the Default project at the top of the console window to: `CompanyName.ProjectName.Repository`
3. Run the command: `Add-Migration NameOfYourMigration --verbose -startupproject CompanyName.ProjectName.WebApi`

**Tests**
Includes Integration and Unit testing projects and a TestUtilities project for sharing logging, in memory database dependencies and AutoMapper mocking between tests.  
 
## Linting with StyleCop
To add StyleCop to a project in the solution, install the StyleCop.Analyzers Nuget Package.
Then, add the custom ruleset to the .csproj file with this code snippet (you may need to adjust the path):

    <PropertyGroup>
    	<CodeAnalysisRuleSet>../CustomStyleCopRules.ruleset</CodeAnalysisRuleSet>
    </PropertyGroup>