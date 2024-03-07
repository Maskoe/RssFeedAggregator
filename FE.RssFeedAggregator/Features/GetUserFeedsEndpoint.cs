using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using MockStuff.Db;

namespace FE.RssFeedAggregator.Features;

public class GetUserFeedsEndpoint : EndpointWithoutRequest<GetUserFeedsResponse>
{
    private readonly AppDbContext dbContext;

    public GetUserFeedsEndpoint(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("feeds/me");
    }

    public override async Task<GetUserFeedsResponse> ExecuteAsync(CancellationToken ct)
    {
        var currentUser = User.ToTokenUser();

        var user = await dbContext.AppUsers
            .Include(x => x.Feeds)
            .FirstAsync(x => x.Id == currentUser.Id);

        var usersFeeds = user.Feeds.Select(newFeed => new FeedDto(newFeed.Id, newFeed.CreatedAt, newFeed.UpdatedAt, newFeed.Name, newFeed.Url)).ToList();


        return new GetUserFeedsResponse(usersFeeds);
    }
}

public record GetUserFeedsResponse(List<FeedDto> Feeds);