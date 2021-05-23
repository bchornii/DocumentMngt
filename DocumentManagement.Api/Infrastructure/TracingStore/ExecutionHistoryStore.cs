using DocumentManagement.Commands.Common.ExecutionHistory;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.Api.Infrastructure.TracingStore
{
    public class ExecutionHistoryStore : IExecutionHistoryStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExecutionHistoryStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        public void Add(string key, object value)
        {
            _httpContextAccessor.HttpContext.Items.Add(key, value);
        }

        /// <inheritdoc/>
        public T Get<T>(string key) where T : class
        {
            return _httpContextAccessor.HttpContext
                .Items.TryGetValue(key, out var r) ? r as T : default(T);
        }
    }
}