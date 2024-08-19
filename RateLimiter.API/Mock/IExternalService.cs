using RateLimiter.API.Model;

namespace RateLimiter.API.Mock;

/// <summary>
/// Interface for external service to fetch user data.
/// </summary>
public interface IExternalService
{
    /// <summary>
    /// Asynchronously generates a list of user data.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of users.</returns>
    Task<List<User>> GetDataAsync();
}

