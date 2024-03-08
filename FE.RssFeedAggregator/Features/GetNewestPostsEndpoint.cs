namespace FE.RssFeedAggregator.Features;

public class GetNewestPostsEndpoint : Endpoint<GetNewestPostsRequest, GetNewestPostsResponse>
{
    private readonly AppDbContext context;

    public GetNewestPostsEndpoint(AppDbContext context)
    {
        this.context = context;
    }

    public override void Configure()
    {
        Post("posts/newest");
    }

    public override async Task<GetNewestPostsResponse> ExecuteAsync(GetNewestPostsRequest req, CancellationToken ct)
    {
        var currentUser = User.ToTokenUser();

        var posts = await context.Posts
            .Where(x => x.Feed.AppUsers.Any(u => u.Id == currentUser.Id))
            .OrderByDescending(x => x.PublishedAt)
            .Take(req.Limit ?? 200)
            .Select(x => new PostDto(x.Title, x.Description, x.PublishedAt, x.Url, x.Feed.Name))
            .ToListAsync();

        return new GetNewestPostsResponse(posts);
    }
}

public record GetNewestPostsRequest(int? Limit);

public record GetNewestPostsResponse(List<PostDto> Posts);

public record PostDto(string Title, string Description, DateTime PublishedAt, string Url, string FeedName);