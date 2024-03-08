namespace FE.RssFeedAggregator.Db;

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