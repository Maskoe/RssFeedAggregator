namespace FE.RssFeedAggregator.Features.Admin;

public class CreateFeedEndpoint : Endpoint<CreateFeedRequest, CreateFeedResponse>
{
    private readonly AppDbContext appDbContext;

    public CreateFeedEndpoint(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public override void Configure()
    {
        Roles("admin");
        Post("feeds");
    }

    public override async Task<CreateFeedResponse> ExecuteAsync(CreateFeedRequest req, CancellationToken ct)
    {
        var newFeed = new Feed { Name = req.Name, Url = req.Url };
        appDbContext.Feeds.Add(newFeed);

        await appDbContext.SaveChangesAsync();

        var feedDto = new FeedDto(newFeed.Id, newFeed.CreatedAt, newFeed.UpdatedAt, newFeed.Name, newFeed.Url);
        return new CreateFeedResponse(feedDto);
    }
}

public record CreateFeedRequest(string Name, string Url);

public record CreateFeedResponse(FeedDto Feed);

public record FeedDto(Guid Id, DateTime CreatedAt, DateTime UpdatedAt, string Name, string Url);