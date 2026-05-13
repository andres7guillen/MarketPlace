using Auth.Application.Responses;
using Auth.Domain.Interfaces;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;

namespace Auth.Application.Features.User.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsersAsync();
        var userResponses = _mapper.Map<IEnumerable<UserResponse>>(users.Value);
        return Result.Success(userResponses);
    }
}
