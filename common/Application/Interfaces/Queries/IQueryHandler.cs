using MediatR;

namespace ItemsAdministration.Common.Application.Interfaces.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
}
