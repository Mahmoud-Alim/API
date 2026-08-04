namespace API.Services;

public interface IPartitionKeyProvider
{
    string GetPartitionKey(HttpContext context);
}
