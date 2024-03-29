namespace FE.RssFeedAggregator.Db;

public class Feed
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; }
    public string Url { get; set; }
    public DateTime? LastFetchedAt { get; set; }

    public List<Subscription> Subscriptions { get; set; } = new();
    public List<AppUser> AppUsers { get; set; } = new();

    public List<Post> Posts { get; set; } = new();
}