using MediatR;

namespace ItemsAdministration.Common.Application.Abstractions.Interfaces.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Unit>
    where TCommand : ICommand
{
}
