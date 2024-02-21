namespace Application.Common.Exceptions;

public class UnableToConnectToDatabaseException : Exception
{
    public UnableToConnectToDatabaseException()
        : base("Unable to connect to database")
    {
    }

    public override string ToString()
    {
        return "Unable to connect to database";
    }
}