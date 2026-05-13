using Auth.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Auth.Domain.Interfaces;

public interface IUserRepository
{
    public Task<Result<IEnumerable<User>>> GetAllUsersAsync();
    public Task<Result<User>> GetUserAsync(string username, string password);
    public Task<Result<User>> CreateUserAsync(User user);
}
