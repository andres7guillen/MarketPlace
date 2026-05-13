using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.CQLRepositories;
using Auth.Infrastructure.Data;
using Cassandra.Mapping;
using CSharpFunctionalExtensions;

namespace Auth.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMapper _usersMapper;
    public UserRepository(IUsersContext usersContext) => _usersMapper = usersContext.GetMapper();

    public async Task<Result<User>> CreateUserAsync(User user)
    {
        var appliedInfo = await _usersMapper.InsertIfNotExistsAsync(user);
        if (appliedInfo.Applied == false)
            return Result.Failure<User>("User already exists");      
        return Result.Success(user);            
    }

    public async Task<Result<IEnumerable<User>>> GetAllUsersAsync() => Result.Success<IEnumerable<User>>(await _usersMapper.FetchAsync<User>());

    public async Task<Result<User>> GetUserAsync(string username, string password)
    {
        var user = await _usersMapper.FirstOrDefaultAsync<User>(UserCQL.GetUserByUserNameCql, username.ToLowerInvariant());
        if (user == null)
            return Result.Failure<User>("User not found");
        else if(user.Password != password)
            return Result.Failure<User>("Invalid password");
        return Result.Success(user);
    }
}
