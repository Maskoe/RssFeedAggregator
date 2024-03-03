using MockStuff.Db;

namespace MockStuff.Handlers;

public class SubscribeToFeedHandler
{
    public static async Task<IResult> Execute(
        HttpContext httpContext,
        AppDbContext appDbContext,
        SubscribeToFeedRequest request,
        CancellationToken ct)
    {
        var currentUser = httpContext.CurrentUser();

        var newSubscription = new Subscription { AppUserId = currentUser.Id, FeedId = request.FeedId };
        appDbContext.Add(newSubscription);

        await appDbContext.SaveChangesAsync();

        return TypedResults.Created();
    }
}

public record SubscribeToFeedRequest(Guid FeedId);

