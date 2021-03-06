namespace AuthR.BusinessLogic.Models.Exceptions;

public class EntityExistsException : Exception
{
    public EntityExistsException(string message) : base(message)
    {
    }

    public EntityExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}