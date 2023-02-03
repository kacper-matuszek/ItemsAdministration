using MediatR;

namespace ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
