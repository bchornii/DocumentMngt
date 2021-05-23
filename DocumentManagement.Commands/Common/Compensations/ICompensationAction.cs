using System.Threading.Tasks;

namespace DocumentManagement.Commands.Common.Compensations
{
    /// <summary>
    /// Encapsulates a compensation action as an object. <see cref="ICompensationActionsFactory"/>
    /// retrieves instance(s) of <see cref="ICompensationAction"/> for operations which needs to be compensated.
    /// </summary>
    public interface ICompensationAction
    {
        /// <summary>
        /// Compensates previously executed operation.
        /// </summary>
        /// <returns></returns>
        Task Compensate();
    }
}