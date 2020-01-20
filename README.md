# internal-core-infrastructure
A sample core and infrastructure project to be used as a base application for new projects. Created with C#, .NET Standard 2.1

## Linting with StyleCop
To add StyleCop to a project in the solution, install the StyleCop.Analyzers Nuget Package.
Then, add the custom ruleset to the .csproj file with this code snippet:

    <PropertyGroup>
    	<CodeAnalysisRuleSet>../CustomStyleCopRules.ruleset</CodeAnalysisRuleSet>
    </PropertyGroup>