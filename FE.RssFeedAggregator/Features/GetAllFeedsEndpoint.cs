using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using MockStuff.Db;

namespace FE.RssFeedAggregator.Features;

public class GetAllFeedsEndpoint : EndpointWithoutRequest<GetAllFeedsResponse>
{
    private readonly AppDbContext context;

    public GetAllFeedsEndpoint(AppDbContext context)
    {
        this.context = context;
    }

    public override void Configure()
    {
        Get("feeds");
    }

    public override async Task<GetAllFeedsResponse> ExecuteAsync(CancellationToken ct)
    {
        var feedDtos = await context.Feeds
            .Select(newFeed => new FeedDto(newFeed.Id, newFeed.CreatedAt, newFeed.UpdatedAt, newFeed.Name, newFeed.Url))
            .ToListAsync();

        return new GetAllFeedsResponse(feedDtos);
    }
}

public record GetAllFeedsResponse(List<FeedDto> Feeds);