using Microsoft.EntityFrameworkCore;
using MockStuff.Db;

namespace MockStuff.Handlers;

public class UnsubscribeFromFeedHandler
{
    public static async Task<IResult> Execute(
        HttpContext httpContext,
        AppDbContext appDbContext,
        UnsubscribeFromFeedRequest request,
        CancellationToken ct)
    {
        await appDbContext.Subscriptions
            .Where(x => x.AppUserId == httpContext.CurrentUser().Id && x.FeedId == request.FeedId)
            .ExecuteDeleteAsync();

        return TypedResults.Ok();
    }
}

public record UnsubscribeFromFeedRequest(Guid FeedId);