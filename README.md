# Rss Feed Aggregator
This is a sample project to showcase FastEndpoints and a minimal Vertical Slice Architecture.  
The idea is MIT licensed from this video: https://www.youtube.com/watch?v=dpXhDzgUSe4

If you're coming here from [breakneck](https://breakneck.dev), this will give you a good idea of the style.

This project manages RSS feeds from blogs. You can add certain blogs to the system and then users can subscribe to those blogs.  
The system constantly synchronizes the newest posts from all blogs.

## Features
- Register as a user and get an api token
- Login with your api token and get a jwt token
- Create RSS feeds from blogs as an admin
- Show all RSS feeds in the system
- Subcribe and unsubscribe to RSS feeds
- Show users the newest posts of followed RSS feeds
- Synchronize the newest posts from all RSS feeds in the system every minute concurrently in the background

## Technical
- AspNetCore Backend API
- Authentication / Authorization with JWT
- Entity Framework Core to store users, feeds and posts
- HangFire to synchronize feeds in the background. Concurrently or sequentially.
- Request Response Architecture

## Getting Started
These are 2 feeds you can play around with: https://blog.boot.dev/index.xml https://wagslane.dev/index.xml

## ToDo
- Docker image to run locally easily, maybe even Aspire?
- Add drawings or minimal frontend to visualize
- Create article or yt video?
- Better Getting Started 

