namespace DocumentManagement.Commands.Common.ExecutionHistory
{
    /// <summary>
    /// Contains method to keep track of execution steps in a form of history.
    /// </summary>
    public interface IExecutionHistoryStore
    {
        /// <summary>
        /// Add record to execution history.
        /// </summary>
        /// <param name="key">Key, representing particular step.</param>
        /// <param name="value">Associated with a step data.</param>
        void Add(string key, object value);

        /// <summary>
        /// Retrieves execution step data.
        /// </summary>
        /// <typeparam name="T">Any reference type.</typeparam>
        /// <param name="key">Key, representing particular step.</param>
        /// <returns></returns>
        T Get<T>(string key) where T : class;
    }
}
