namespace TogetherBoardsApp.Backend.Application.Exceptions;

public class EmailOrPasswordIncorrectException : Exception
{
    public EmailOrPasswordIncorrectException(string email)
        : base($"User account with email {email} was not found or the given password is incorrect")
    {
    }
}