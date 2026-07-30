namespace Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public string EntityName { get; }
    public object Key { get; }

    public NotFoundException(string entityName, object key)
        : base($"{entityName} with identifier ({key}) was not found.")
    {
        EntityName = entityName;
        Key = key;
    }
}


