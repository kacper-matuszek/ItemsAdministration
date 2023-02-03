using MediatR;

namespace ItemsAdministration.Common.Application.Interfaces.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
