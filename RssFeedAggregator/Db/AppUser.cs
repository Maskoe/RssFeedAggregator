namespace MockStuff.Db;

public class AppUser
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; }
    public string ApiKey { get; set; }

    public List<Subscription> Subscriptions { get; set; } = new();
    public List<Feed> Feeds { get; set; } = new();
}