namespace Application.Common.Exceptions;

public class InvalidDbContextException : Exception
{
    public InvalidDbContextException() : base("Invalid DbContext")
    {
    }

    public override string ToString()
    {
        return "Invalid DbContext";
    }
}