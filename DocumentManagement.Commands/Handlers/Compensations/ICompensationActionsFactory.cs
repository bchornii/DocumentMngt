using DocumentManagement.Commands.Handlers.Compensations.Commands;

namespace DocumentManagement.Commands.Handlers.Compensations
{
    public interface ICompensationActionsFactory
    {
        bool TryGet(string routeName, out ICompensationAction action);
    }
}
