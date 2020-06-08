namespace CompanyName.ProjectName.Core.Enums.Logging
{
    public enum LoggingEventType
    {
        /// <summary>
        /// Contain the most detailed messages. These messages may contain sensitive app data.
        /// These messages are disabled by default and should not be enabled in production.
        /// </summary>
        Trace,

        /// <summary>
        /// For debugging and development. Use with caution in production due to the high volume.
        /// </summary>
        Debug,

        /// <summary>
        /// Tracks the general flow of the app. May have long-term value.
        /// </summary>
        Information,

        /// <summary>
        /// For abnormal or unexpected events. Typically includes errors or conditions that don't
        /// cause the app to fail.
        /// </summary>
        Warning,

        /// <summary>
        /// For errors and exceptions that cannot be handled. These messages indicate a failure in
        /// the current operation or request, not an app-wide failure.
        /// </summary>
        Error,

        /// <summary>
        /// For failures that require immediate attention. Examples: data loss scenarios, out of disk space.
        /// </summary>
        Critical,

        /// <summary>
        /// Specifies that a logging category should not write any messages.
        /// </summary>
        None
    }
}
