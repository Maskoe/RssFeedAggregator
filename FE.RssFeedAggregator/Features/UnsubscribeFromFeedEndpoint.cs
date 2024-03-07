using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using MockStuff.Db;

namespace FE.RssFeedAggregator.Features;

public class UnsubscribeFromFeedEndpoint : Endpoint<UnsubscribeFromFeedRequest>
{
    private readonly AppDbContext context;

    public UnsubscribeFromFeedEndpoint(AppDbContext context)
    {
        this.context = context;
    }

    public override void Configure()
    {
        Post("feeds/unsubscribe");
    }

    public override async Task HandleAsync(UnsubscribeFromFeedRequest req, CancellationToken ct)
    {
        await context.Subscriptions
            .Where(x => x.AppUserId == User.ToTokenUser().Id && x.FeedId == req.FeedId)
            .ExecuteDeleteAsync();
    }
}

public record UnsubscribeFromFeedRequest(Guid FeedId);