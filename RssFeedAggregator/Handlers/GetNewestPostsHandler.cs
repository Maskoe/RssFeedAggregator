using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockStuff.Db;

namespace MockStuff.Handlers;

public class GetNewestPostsHandler
{
    // this works much better in FE, RequestBinding.
    public static async Task<IResult> Execute(HttpContext httpContext, AppDbContext dbContext, CancellationToken ct)
    {
        var currentUser = httpContext.CurrentUser();
        var limit = int.TryParse(httpContext.Request.Query["limit"], out var parsedInt) ? parsedInt : 20;

        var user = await dbContext.AppUsers
            .Include(x => x.Feeds)
            .ThenInclude(x => x.Posts.Take(limit))
            .FirstAsync(x => x.Id == currentUser.Id);

        var posts = user.Feeds
            .SelectMany(x => x.Posts)
            .OrderByDescending(x => x.PublishedAt)
            .Select(x => new PostDto(x.Id, x.Title, x.Description, x.PublishedAt, x.Url, x.Feed.Name));
        
        return TypedResults.Ok(posts);
    }
}

public record PostDto(Guid Id, string Title, string Description, DateTime PublishedAt, string Url, string FeedName);