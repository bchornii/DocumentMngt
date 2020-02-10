using System.Threading.Tasks;

namespace DocumentManagement.Commands.Handlers.Compensations.Commands
{
    public interface ICompensationAction
    {
        Task Compensate();
    }
}