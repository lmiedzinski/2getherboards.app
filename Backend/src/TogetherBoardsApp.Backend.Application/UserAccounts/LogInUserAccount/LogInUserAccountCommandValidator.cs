using FluentValidation;

namespace TogetherBoardsApp.Backend.Application.UserAccounts.LogInUserAccount;

internal sealed class LogInUserAccountCommandValidator : AbstractValidator<LogInUserAccountCommand>
{
    public LogInUserAccountCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}