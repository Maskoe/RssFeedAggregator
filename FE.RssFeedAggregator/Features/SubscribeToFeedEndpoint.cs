namespace FE.RssFeedAggregator.Features;

public class SubscribeToFeedEndpoint : Endpoint<SubscribeToFeedRequest>
{
    private readonly AppDbContext context;

    public SubscribeToFeedEndpoint(AppDbContext context)
    {
        this.context = context;
    }

    public override void Configure()
    {
        Post("feeds/subscribe");
    }

    public override async Task HandleAsync(SubscribeToFeedRequest req, CancellationToken ct)
    {
        var currentUser = User.ToTokenUser();

        var newSubscription = new Subscription { AppUserId = currentUser.Id, FeedId = req.FeedId };
        context.Add(newSubscription);

        await context.SaveChangesAsync();
    }
}

public record SubscribeToFeedRequest(Guid FeedId);