namespace CRM.Data.Common.Exceptions;

public class NoUserContextException : Exception
{
    public NoUserContextException()
    {
    }

    public NoUserContextException(string message)
        : base(message)
    {
    }

    public NoUserContextException(string message, Exception inner)
        : base(message, inner)
    {
    }
}