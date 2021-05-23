namespace DocumentManagement.Commands.Common.Compensations
{
    public interface ICompensationActionsFactory
    {
        /// <summary>
        /// Retrieves <see cref="ICompensationAction"/> instance which should be invoked
        /// in order to revert changes of the particular operation.
        /// </summary>
        /// <param name="routeName">Route of the operation being compensated. In a simplest case command handler type name.</param>
        /// <param name="action">Compensation action for the operation at <paramref name="routeName"/>.</param>
        /// <returns></returns>
        bool TryGet(string routeName, out ICompensationAction action);
    }
}
