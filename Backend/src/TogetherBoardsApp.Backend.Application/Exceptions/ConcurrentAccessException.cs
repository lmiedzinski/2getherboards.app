namespace TogetherBoardsApp.Backend.Application.Exceptions;

public class ConcurrentAccessException : Exception
{
    public ConcurrentAccessException(Exception innerException)
        : base("Original data was modified before saving changes", innerException)
    {
    }
}