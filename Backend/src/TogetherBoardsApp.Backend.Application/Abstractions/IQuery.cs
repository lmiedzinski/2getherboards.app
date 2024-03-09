using MediatR;

namespace TogetherBoardsApp.Backend.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}