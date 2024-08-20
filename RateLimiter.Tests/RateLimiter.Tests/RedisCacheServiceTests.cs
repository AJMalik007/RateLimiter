using Moq;
using RateLimiter.API.Services.Redis;
using StackExchange.Redis;

namespace RateLimiter.Tests
{
    public class RedisCacheServiceTests
    {
        private readonly Mock<IConnectionMultiplexer> _redisMock;
        private readonly Mock<IDatabase> _dbMock;
        private readonly RedisCacheService _redisCacheService;

        public RedisCacheServiceTests()
        {
            _redisMock = new Mock<IConnectionMultiplexer>();
            _dbMock = new Mock<IDatabase>();
            _redisMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_dbMock.Object);
            _redisCacheService = new RedisCacheService(_redisMock.Object);
        }

        [Fact]
        public async Task GetCacheValueAsync_ShouldReturnCachedValue()
        {
            // Arrange
            var key = "test-key";
            var expectedValue = "test-value";
            _dbMock.Setup(db => db.StringGetAsync(key, It.IsAny<CommandFlags>())).ReturnsAsync(expectedValue);

            // Act
            var actualValue = await _redisCacheService.GetCacheValueAsync(key);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async Task GetCacheValueAsync_ShouldHandleRedisConnectionFailure()
        {
            // Arrange
            var key = "test-key";
            _redisMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Throws(new RedisConnectionException(ConnectionFailureType.UnableToResolvePhysicalConnection, "Simulated connection failure"));

            // Act & Assert
            await Assert.ThrowsAsync<RedisConnectionException>(() => _redisCacheService.GetCacheValueAsync(key));
        }

        [Fact]
        public async Task SetCacheValueAsync_ShouldHandleRedisConnectionFailure()
        {
            // Arrange
            var key = "test-key";
            var value = "test-value";
            var expiration = TimeSpan.FromMinutes(5);
            _redisMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Throws(new RedisConnectionException(ConnectionFailureType.UnableToResolvePhysicalConnection, "Simulated connection failure"));

            // Act & Assert
            await Assert.ThrowsAsync<RedisConnectionException>(() => _redisCacheService.SetCacheValueAsync(key, value, expiration));
        }
    }
}
