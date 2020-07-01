# .NET Core Domain Template
A sample .NET Core solution to be used as a template for new projects.

## Localhost Spinup Instructions
- Clone this repository and open it using Visual Studio 2019.
- If you aren't on the latest update for Visual Studio 2019 then you should perform the update to get .NET Core 3.1. In the toolbar, Go to Help -> Check for updates -> Update.
- If it isn't already, set the WebApi project as the sole startup project. Right click the solution and select properties. Under "Common Properties" select Startup Project. Then select "Single startup project" followed by the WebApi project from the drop-down list.
- The database is a localdb so once the project is run for the first time the database will be created using Entity Framework code-first migrations and seeded with test data. 

## CompanyName.ProjectName.Core
A core project to hold domain models, aggregates, interfaces, constants etc.

## CompanyName.ProjectName.Infrastructure
A project to hold services and business logic.

## CompanyName.ProjectName.Mapping
Configurations for dependency injection and auto-mapper to be shared across projects.

## CompanyName.ProjectName.Repository
This repository uses Entity Framework Core and the Repository Pattern for CRUD operations to a database.

### Running Code First Migrations
1. Open the Package Manager Console window in Visual Studio.
2. Set the Default project at the top of the console window to: `CompanyName.ProjectName.Repository`
3. Run the command: `Add-Migration NameOfYourMigration --verbose -startupproject CompanyName.ProjectName.WebApi`

## CompanyName.ProjectName.WebApi
A RESTful Web Api project for communication with a UI layer. 
 
## Linting with StyleCop
To add StyleCop to a project in the solution, install the StyleCop.Analyzers Nuget Package.
Then, add the custom ruleset to the .csproj file with this code snippet:

    <PropertyGroup>
    	<CodeAnalysisRuleSet>../CustomStyleCopRules.ruleset</CodeAnalysisRuleSet>
    </PropertyGroup>