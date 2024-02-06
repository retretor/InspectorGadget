namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object entityId)
        : base($"Entity {entityName} with id {entityId} was not found.")
    {
    }
}