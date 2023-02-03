using ItemsAdministration.Common.Application.Interfaces.Commands;
using ItemsAdministration.Common.Application.Interfaces.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace ItemsAdministration.Common.Application.Interfaces.Dispatchers;

public interface IDispatcher
{
    Task Send(ICommand command, CancellationToken cancellationToken = default);
    Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}
