using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Users.CreateUser;

public class CreateUserAccountCommandHandler : IRequestHandler<CreateUserAccountCommand, Result<string>>
{ 
    private readonly IUserRepository _userRepository;

    public CreateUserAccountCommandHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<Result<string>> Handle(CreateUserAccountCommand request, CancellationToken cancellationToken)
    {
        var userResult = Auth.Domain.Entities.User.Create(request.UserName, request.Id, request.Password, request.Role, request.Email, request.Name, request.SurName, request.Birthday, 
            request.CreationDate, request.Age, request.ProfilePhoto, request.IsPremium);
        if (userResult.IsFailure)
        {
            return Result.Failure<string>(userResult.Error);
        }

        var user = await _userRepository.CreateUserAsync(userResult.Value);
        return Result.Success(user.Value.UserName);
    }
}
