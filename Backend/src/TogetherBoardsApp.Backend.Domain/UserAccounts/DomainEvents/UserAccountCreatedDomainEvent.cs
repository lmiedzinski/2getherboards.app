using TogetherBoardsApp.Backend.Domain.Abstractions;

namespace TogetherBoardsApp.Backend.Domain.UserAccounts.DomainEvents;

public record UserAccountCreatedDomainEvent(Guid Id, UserAccountId UserAccountId) : DomainEvent(Id);