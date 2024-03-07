namespace TogetherBoardsApp.Backend.Domain.Abstractions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }
}