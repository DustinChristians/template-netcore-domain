# internal-core-infrastructure
A sample core and infrastructure project to be used as a base application for new projects. Created with C#, .NET Standard 2.1

## Linting with StyleCop
Add a custom ruleset to a .csproj file with this code snippet:

    <PropertyGroup>
    	<CodeAnalysisRuleSet>../CustomStyleCopRules.ruleset</CodeAnalysisRuleSet>
    </PropertyGroup>