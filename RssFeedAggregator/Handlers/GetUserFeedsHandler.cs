using Microsoft.EntityFrameworkCore;
using MockStuff.Db;

namespace MockStuff.Handlers;

public class GetUserFeedsHandler
{
    public static async Task<IResult> Execute(HttpContext httpContext, AppDbContext dbContext, CancellationToken ct)
    {
        var currentUser = httpContext.CurrentUser();

        var subscriptions = await dbContext.Subscriptions.Where(x => x.AppUserId == currentUser.Id).ToListAsync(); // this is what lane returned
        var user = await dbContext.AppUsers
            .Include(x => x.Feeds)
            .FirstAsync(x => x.Id == currentUser.Id);

        var usersFeeds = user.Feeds.Select(newFeed => new FeedDto(newFeed.Id, newFeed.CreatedAt, newFeed.UpdatedAt, newFeed.Name, newFeed.Url));

        return TypedResults.Ok(usersFeeds);
    }
}