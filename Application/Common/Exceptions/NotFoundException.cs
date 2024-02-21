namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    private readonly string _entityName;
    private readonly object _entityId;

    public NotFoundException(string entityName, object entityId)
        : base(BuildErrorMessage(entityName, entityId))
    {
        _entityName = entityName;
        _entityId = entityId;
    }

    private static string BuildErrorMessage(string entityName, object entityId)
    {
        return $"Entity {entityName} with id {entityId} was not found.";
    }

    public override string ToString()
    {
        return BuildErrorMessage(_entityName, _entityId);
    }
}