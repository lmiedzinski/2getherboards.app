using MediatR;

namespace TogetherBoardsApp.Backend.Domain.Abstractions;

public record DomainEvent(Guid Id) : INotification;