using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FE.RssFeedAggregator.Features.Admin;
using Hangfire;

var bld = WebApplication.CreateBuilder();
bld.Services.AddFastEndpoints();
bld.Services.SwaggerDocument();

bld.Services.AddHttpClient();
bld.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseNpgsql("User ID=postgres; Password=postgres; Database=rss; Server=localhost; Port=5432; Pooling=false; Include Error Detail=true;");
    //options.EnableSensitiveDataLogging();
});

bld.Services.AddAuthenticationJwtBearer(x => x.SigningKey = "MySuperSecretJwtSecretDontTellAnyone");
bld.Services.AddAuthorization();

bld.Services.AddHangfire(x => x.UseInMemoryStorage());
bld.Services.AddHangfireServer();

bld.Services.AddTransient<SyncFeeds>();

var app = bld.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();

// Automatically migrate database on start up
var sp = app.Services.CreateScope().ServiceProvider;
var dbContext = sp.GetRequiredService<AppDbContext>();
await dbContext.Database.MigrateAsync();

// Set up background tasks
var syncFeeds = sp.GetRequiredService<SyncFeeds>();
var recurringJobs = sp.GetRequiredService<IRecurringJobManager>();
// recurringJobs.AddOrUpdate("sync-feeds-sequentially", () => syncFeeds.SyncSequentially(), Cron.Minutely);
//recurringJobs.AddOrUpdate("sync-feeds-concurrently", () => syncFeeds.SyncConcurrently(), Cron.Minutely);
// recurringJobs.AddOrUpdate("100x", () => syncFeeds.DoSomething100Times(), Cron.Minutely);

app.Run();