namespace MockStuff.Db;

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

public class Post
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime PublishedAt { get; set; }
    public string Url { get; set; }

    public Guid FeedId { get; set; }
    public Feed Feed { get; set; }
}