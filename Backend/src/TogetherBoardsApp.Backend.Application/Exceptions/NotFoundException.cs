namespace TogetherBoardsApp.Backend.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string objectName, string? objectIdentifier = null)
        : base(string.IsNullOrEmpty(objectIdentifier)
            ? $"{objectName} was not found"
            : $"{objectName} with identifier {objectIdentifier} was not found")
    {
    }
}