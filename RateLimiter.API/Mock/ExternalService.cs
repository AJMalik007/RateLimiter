using Bogus;
using RateLimiter.API.Model;

namespace RateLimiter.API.Mock;

/// <summary>
/// Mock external service to generate user data.
/// </summary>
public class ExternalService : IExternalService
{
    /// <summary>
    /// Asynchronously generates a list of user data.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of users.</returns>
    public async Task<List<User>> GetDataAsync()
    {
        // Use Bogus to generate fake user data
        var userFaker = new Faker<User>()
           .RuleFor(u => u.Id, f => f.IndexFaker + 1)
           .RuleFor(u => u.FirstName, f => f.Name.FirstName())
           .RuleFor(u => u.LastName, f => f.Name.LastName())
           .RuleFor(u => u.Email, f => f.Internet.Email())
           .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber());

        // Generate 10 users
        var users = userFaker.Generate(10);

        // Added delay to simulate the API call
        await Task.Delay(5000);

        return users;
    }
}

