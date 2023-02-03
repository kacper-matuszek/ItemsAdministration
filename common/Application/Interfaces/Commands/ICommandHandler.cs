using MediatR;

namespace ItemsAdministration.Common.Application.Interfaces.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Unit>
    where TCommand : ICommand
{
}
