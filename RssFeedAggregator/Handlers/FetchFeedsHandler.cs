using System.Xml.Serialization;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using MockStuff.Db;

namespace MockStuff.Handlers;

public class FetchFeedsHandler
{
    private readonly HttpClient httpClient;
    private readonly AppDbContext dbContext;

    public FetchFeedsHandler(HttpClient httpClient, AppDbContext dbContext)
    {
        this.httpClient = httpClient;
        this.dbContext = dbContext;
    }

    public async Task FetchFeedsSequentially()
    {
        // Get RSS feeds from db
        var dbFeeds = await dbContext.Feeds
            .Include(x => x.Posts)
            .ToListAsync();

        foreach (var dbFeed in dbFeeds)
        {
            // Scrape newest posts from the internet
            var xmlString = await httpClient.GetStringAsync(dbFeed.Url);

            var serializer = new XmlSerializer(typeof(RssRoot));
            using var reader = new StringReader(xmlString);

            var deserializedFeed = (RssRoot)serializer.Deserialize(reader);


            // Figure out which posts are new/missing and map them to db entity
            var newPosts = deserializedFeed.channel.items
                .Where(scrapedPost => dbFeed.Posts.All(dbPost => scrapedPost.link != dbPost.Url))
                .Select(scrapedPost => new Post
                {
                    Title = scrapedPost.title,
                    Url = scrapedPost.link,
                    Description = scrapedPost.description,
                    PublishedAt = DateTime.Parse(scrapedPost.pubDate).ToUniversalTime(),
                });

            // update feed in the db
            dbFeed.Posts.AddRange(newPosts);
            dbFeed.LastFetchedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
        }

        // ToDo: Endpoitn that returns all the newest Posts for the user with a limit query param
        // Thunderclient looks way better than insomnia, does it exist as standalone app?
    }

    public async Task FetchFeedsConcurrently()
    {
        var feedIds = await dbContext.Feeds.Select(x => x.Id).ToListAsync();

        foreach (var feedId in feedIds)
            BackgroundJob.Enqueue(() => FetchSingleFeed(feedId));
    }

    public async Task FetchSingleFeed(Guid feedId)
    {
        var dbFeed = await dbContext.Feeds
            .Include(x => x.Posts)
            .FirstAsync(x => x.Id == feedId);

        // Console.WriteLine("Starting fetch feed " + dbFeed.Name);
        // await Task.Delay(3000);

        // Scrape newest posts from the internet
        var xmlString = await httpClient.GetStringAsync(dbFeed.Url);

        var serializer = new XmlSerializer(typeof(RssRoot));
        using var reader = new StringReader(xmlString);

        var deserializedFeed = (RssRoot)serializer.Deserialize(reader);


        // Figure out which posts are new/missing and map them to db entity
        var newPosts = deserializedFeed.channel.items
            .Where(scrapedPost => dbFeed.Posts.All(dbPost => scrapedPost.link != dbPost.Url))
            .Select(scrapedPost => new Post
            {
                Title = scrapedPost.title,
                Url = scrapedPost.link,
                Description = scrapedPost.description,
                PublishedAt = DateTime.Parse(scrapedPost.pubDate).ToUniversalTime(),
            });

        // update feed in the db
        dbFeed.Posts.AddRange(newPosts);
        dbFeed.LastFetchedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        // Console.WriteLine("Finished fetch feed " + dbFeed.Name);
    }


    // foreach (var scrapedPost in deserializedFeed.channel.items)
    // {
    //     var existingDbPost = dbFeed.Posts.FirstOrDefault(x => x.Url == scrapedPost.link);
    //     if (existingDbPost is null)
    //     {
    //         var newDbPost = new Post
    //         {
    //             Title = scrapedPost.title,
    //             Url = scrapedPost.link,
    //             Description = scrapedPost.description,
    //             PublishedAt = DateTime.Parse(scrapedPost.pubDate).ToUniversalTime(),
    //         };
    //         dbFeed.Posts.Add(newDbPost);
    //     }
    //
    //     var postIsMissingInDb = !dbFeed.Posts.Any(x => x.Url == scrapedPost.link);
    //     if (postIsMissingInDb)
    //     {
    //         var newDbPost = new Post
    //         {
    //             Title = scrapedPost.title,
    //             Url = scrapedPost.link,
    //             Description = scrapedPost.description,
    //             PublishedAt = DateTime.Parse(scrapedPost.pubDate).ToUniversalTime(),
    //         };
    //         dbFeed.Posts.Add(newDbPost);
    //     }
    //     
    //     var postAlreadyExists = dbFeed.Posts.Any(x => x.Url == scrapedPost.link);
    //     if(postAlreadyExists)
    //         continue;
    //     
    //     var newDbPost = new Post
    //     {
    //         Title = scrapedPost.title,
    //         Url = scrapedPost.link,
    //         Description = scrapedPost.description,
    //         PublishedAt = DateTime.Parse(scrapedPost.pubDate).ToUniversalTime(),
    //     };
    //     dbFeed.Posts.Add(newDbPost);
    // }

    public async static Task DoSomething()
    {
        Console.WriteLine("Starting at " + DateTime.Now.ToLongTimeString());
        await Task.Delay(2000);
        Console.WriteLine("Finished at " + DateTime.Now.ToLongTimeString());
    }

    public async static Task DoSomething100Times()
    {
        for (int i = 0; i < 100; i++)
        {
            BackgroundJob.Enqueue(() => DoSomething());
        }
    }
}

[XmlRoot("rss")]
public class RssRoot
{
    public RssFeed channel { get; set; }
}

public class RssFeed
{
    public string title { get; set; }
    public string link { get; set; }
    public string description { get; set; }
    public string language { get; set; }

    [XmlElement("item")] public List<RssItem> items { get; set; }
}

public class RssItem
{
    public string title { get; set; }
    public string link { get; set; }
    public string description { get; set; }
    public string pubDate { get; set; }
}