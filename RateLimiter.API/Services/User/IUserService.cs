using RateLimiter.API.Model;

namespace RateLimiter.API.Services;

/// <summary>
/// Interface for user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Asynchronously retrieves all users.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of users.</returns>
    Task<List<User>> GetAllAsync();
}
