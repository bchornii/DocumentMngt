using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using DocumentManagement.Commands.Common.Compensations;

namespace DocumentManagement.Api.Infrastructure.Interceptors
{
    public class ExceptionInterceptor : IInterceptor
    {
        private readonly ICompensationActionsFactory _actionsFactory;

        public ExceptionInterceptor(ICompensationActionsFactory actionsFactory)
        {
            _actionsFactory = actionsFactory;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            var method = invocation.MethodInvocationTarget;
            var isAsync = method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;
            if (isAsync && typeof(Task).IsAssignableFrom(method.ReturnType))
            {
                invocation.ReturnValue = InterceptAsync((dynamic)invocation.ReturnValue, invocation);
            }
        }

        private async Task InterceptAsync(Task task, IInvocation invocation)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch
            {
                await CompensateCommandExecution(invocation);
                throw;
            }
        }

        private async Task<T> InterceptAsync<T>(Task<T> task, IInvocation invocation)
        {
            try
            {
                T result = await task.ConfigureAwait(false);
                return result;
            }
            catch
            {
                await CompensateCommandExecution(invocation);
                throw;
            }
        }

        private Task CompensateCommandExecution(IInvocation invocation)
        {
            if (_actionsFactory.TryGet(invocation.TargetType.Name, out var action))
            {
                return action.Compensate();
            }
            return Task.CompletedTask;
        }
    }
}
