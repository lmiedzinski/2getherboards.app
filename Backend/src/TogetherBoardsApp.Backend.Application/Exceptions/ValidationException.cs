namespace TogetherBoardsApp.Backend.Application.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(IReadOnlyCollection<ValidationError> errors) : base("Validation failed")
    {
        Errors = errors;
    }

    public IReadOnlyCollection<ValidationError> Errors { get; }
}

public sealed record ValidationError(string PropertyName, string ErrorMessage);