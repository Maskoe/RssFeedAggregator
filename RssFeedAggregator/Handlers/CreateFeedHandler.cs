using MockStuff.Db;

namespace MockStuff.Handlers;

public class CreateFeedHandler
{
    public static async Task<IResult> Execute(
        AppDbContext appDbContext,
        CreateFeedRequest request,
        CancellationToken ct)
    {
        var newFeed = new Feed { Name = request.Name, Url = request.Url};
        appDbContext.Feeds.Add(newFeed);

        await appDbContext.SaveChangesAsync();

        var feedDto = new FeedDto(newFeed.Id, newFeed.CreatedAt, newFeed.UpdatedAt, newFeed.Name, newFeed.Url);

        return TypedResults.Ok(feedDto);
    }
}

public record CreateFeedRequest(string Name, string Url);

public record FeedDto(Guid Id, DateTime CreatedAt, DateTime UpdatedAt, string Name, string Url);