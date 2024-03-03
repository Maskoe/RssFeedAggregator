using Microsoft.EntityFrameworkCore;
using MockStuff.Db;

namespace MockStuff.Handlers;

public class GetFeedsHandler
{
    public static async Task<IResult> Execute(AppDbContext appDbContext)
    {
        var allFeeds = await appDbContext.Feeds.ToListAsync();

        var feedDtos = allFeeds.Select(newFeed => new FeedDto(newFeed.Id, newFeed.CreatedAt, newFeed.UpdatedAt, newFeed.Name, newFeed.Url));

        return TypedResults.Ok(feedDtos);
    }
}