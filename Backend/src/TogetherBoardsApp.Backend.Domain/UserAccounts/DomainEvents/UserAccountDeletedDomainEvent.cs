using TogetherBoardsApp.Backend.Domain.Abstractions;

namespace TogetherBoardsApp.Backend.Domain.UserAccounts.DomainEvents;

public sealed record UserAccountDeletedDomainEvent(Guid Id, UserAccountId UserAccountId) : DomainEvent(Id);