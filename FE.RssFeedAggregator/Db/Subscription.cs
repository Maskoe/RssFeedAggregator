namespace FE.RssFeedAggregator.Db;

public class Subscription
{
    public Guid Id { get; set; }

    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public Guid FeedId { get; set; }
    public Feed Feed { get; set; }
}