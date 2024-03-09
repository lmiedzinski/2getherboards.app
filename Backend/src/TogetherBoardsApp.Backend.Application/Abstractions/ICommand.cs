using MediatR;

namespace TogetherBoardsApp.Backend.Application.Abstractions;

public interface ICommand : IRequest, ICommandBase
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>, ICommandBase
{
}

public interface ICommandBase
{
}