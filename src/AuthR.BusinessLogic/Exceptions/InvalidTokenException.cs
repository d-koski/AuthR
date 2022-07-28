namespace AuthR.BusinessLogic.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(string message) : base(message)
    {
    }
}