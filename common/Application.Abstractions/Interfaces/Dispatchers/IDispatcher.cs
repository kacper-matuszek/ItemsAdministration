using ItemsAdministration.Common.Application.Abstractions.Interfaces.Commands;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace ItemsAdministration.Common.Application.Abstractions.Interfaces.Dispatchers;

public interface IDispatcher
{
    Task Send(ICommand command, CancellationToken cancellationToken = default);
    Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}
