namespace Application.Common.Exceptions;

public class AccessDeniedException : Exception
{
    public AccessDeniedException()
        : base("Access denied.")
    {
    }
    
    public override string ToString()
    {
        return "Access denied.";
    }
}