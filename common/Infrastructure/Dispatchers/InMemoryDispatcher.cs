using ItemsAdministration.Common.Application.Abstractions.Interfaces.Commands;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Dispatchers;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ItemsAdministration.Common.Infrastructure.Dispatchers;

public sealed class InMemoryDispatcher : IDispatcher
{
    private readonly IMediator _mediator;

    public InMemoryDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Send(ICommand command, CancellationToken cancellationToken = default) =>
        _mediator.Send(command, cancellationToken);

    public Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default) =>
        _mediator.Send(query, cancellationToken);
}
