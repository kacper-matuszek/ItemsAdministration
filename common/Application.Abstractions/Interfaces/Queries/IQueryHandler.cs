using MediatR;

namespace ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
}
