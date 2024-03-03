using Hangfire;
using Microsoft.EntityFrameworkCore;
using MockStuff.Db;
using MockStuff.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("User ID=postgres; Password=postgres; Database=lmao; Server=localhost; Port=5432; Pooling=false; Include Error Detail=true;");
    options.EnableSensitiveDataLogging();
});

builder.Services.AddHttpClient();
builder.Services.AddProblemDetails();
builder.Services.AddHangfire(x => x.UseInMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddTransient<FetchFeedsHandler>();


var app = builder.Build();
app.UseExceptionHandler();

var sp = app.Services.CreateScope().ServiceProvider;
var dbContext = sp.GetRequiredService<AppDbContext>();
await dbContext.Database.MigrateAsync();

var v1 = app.MapGroup("v1");

v1.MapPost("/register", CreateUserHandler.Execute);
v1.MapGet("/user", GetUserHandler.Execute).AddEndpointFilter<AuthFilter>();

v1.MapPost("/feeds", CreateFeedHandler.Execute).AddEndpointFilter<AuthFilter>();
v1.MapGet("/feeds", GetFeedsHandler.Execute).AddEndpointFilter<AuthFilter>();

v1.MapPost("/subscribe-to-feed", SubscribeToFeedHandler.Execute).AddEndpointFilter<AuthFilter>();
v1.MapPost("/unsubscribe-from-feed", UnsubscribeFromFeedHandler.Execute).AddEndpointFilter<AuthFilter>();
v1.MapGet("/my-feeds", GetUserFeedsHandler.Execute).WithAuth();

v1.MapPost("/go", FetchFeedsHandler.FetchAll);

// ToDo fetch every 10min

app.Run();
