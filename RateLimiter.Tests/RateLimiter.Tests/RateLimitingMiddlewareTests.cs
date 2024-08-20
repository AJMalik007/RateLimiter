using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RateLimiter.API.Middleware;
using RateLimiter.API.Services.RateLimit;
using System.Net;

namespace RateLimiter.Tests;

public class RateLimitingMiddlewareTests
{
    private readonly Mock<IRateLimitingService> _rateLimitingServiceMock;
    private readonly TestServer _server;
    private readonly HttpClient _client;

    public RateLimitingMiddlewareTests()
    {
        _rateLimitingServiceMock = new Mock<IRateLimitingService>();

        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(_rateLimitingServiceMock.Object);
                services.AddTransient<RateLimitingMiddleware>();
            })
            .Configure(app =>
            {
                app.UseMiddleware<RateLimitingMiddleware>();
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Hello, world!");
                });
            });

        _server = new TestServer(builder);
        _client = _server.CreateClient();
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn400_WhenApiKeyIsMissing()
    {
        var response = await _client.GetAsync("/");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn429_WhenClientRequestLimitExceeded()
    {
        _rateLimitingServiceMock
            .Setup(service => service.IsRequestAllowedAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(false);

        var request = new HttpRequestMessage(HttpMethod.Get, "/");
        request.Headers.Add("X-API-KEY", "test-client");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.TooManyRequests, response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn200_WhenRequestIsAllowed()
    {
        _rateLimitingServiceMock
            .Setup(service => service.IsRequestAllowedAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(true);

        var request = new HttpRequestMessage(HttpMethod.Get, "/");
        request.Headers.Add("X-API-KEY", "test-client");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
