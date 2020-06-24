namespace CompanyName.ProjectName.Core.Types
{
    public enum DependencyTypes
    {
        /// <summary>
        /// Created once per request within the scope.
        /// </summary>
        Scoped = 1,

        /// <summary>
        /// A single instance throughout the application.
        /// </summary>
        Singleton = 2,

        /// <summary>
        /// created each time they are requested.
        /// </summary>
        Transient = 3
    }
}
