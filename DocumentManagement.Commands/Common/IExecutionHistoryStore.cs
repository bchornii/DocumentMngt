namespace DocumentManagement.Commands.Common
{
    public interface IExecutionHistoryStore
    {
        void Add(string key, object value);
        T Get<T>(string key) where T : class;
    }
}
