using System;

public class WeavingException : Exception
{
    public WeavingException()
    {
    }

    public WeavingException(string message)
        : base(message)
    {
    }

    public WeavingException(string message, Exception inner)
        : base(message, inner)
    {
    }
}