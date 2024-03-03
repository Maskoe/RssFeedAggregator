using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockStuff.Db;

namespace MockStuff.Handlers;

public class GetNewestPostsHandler
{
    // this works much better in FE, RequestBinding.
    public static async Task<IResult> Execute(
        [AsParameters] GetNewestPostsRequest req,
        HttpContext httpContext, 
        AppDbContext dbContext, 
        CancellationToken ct)
    {
        var currentUser = httpContext.CurrentUser();

        var posts = await dbContext.Set<Post>()
            .Where(x => x.Feed.AppUsers.Any(u => u.Id == currentUser.Id))
            .OrderByDescending(x => x.PublishedAt)
            .Take(req.Limit ?? 200)
            .Select(x => new PostDto(x.Id, x.Title, x.Description, x.PublishedAt, x.Url, x.Feed.Name))
            .ToListAsync();
        
        return TypedResults.Ok(posts);
    }
}

public record GetNewestPostsRequest(int? Limit);
public record PostDto(Guid Id, string Title, string Description, DateTime PublishedAt, string Url, string FeedName);