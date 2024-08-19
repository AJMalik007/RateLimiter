using Bogus;
using RateLimiter.API.Model;

namespace RateLimiter.API.Mock;


public interface IExternalService
{
    Task<List<User>> GetDataAsync();
}

public class ExternalService : IExternalService
{
    public async Task<List<User>> GetDataAsync()
    {
        var userFaker = new Faker<User>()
           .RuleFor(u => u.Id, f => f.IndexFaker + 1)
           .RuleFor(u => u.FirstName, f => f.Name.FirstName())
           .RuleFor(u => u.LastName, f => f.Name.LastName())
           .RuleFor(u => u.Email, f => f.Internet.Email())
           .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber());

        var users = userFaker.Generate(10);

        return users;
    }
}
