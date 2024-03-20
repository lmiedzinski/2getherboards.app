using FluentValidation;

namespace TogetherBoardsApp.Backend.Application.UserAccounts.RefreshUserAccountToken;

internal sealed class RefreshUserAccountTokenCommandValidator : AbstractValidator<RefreshUserAccountTokenCommand>
{
    public RefreshUserAccountTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}