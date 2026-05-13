using CSharpFunctionalExtensions;
using MediatR;

namespace Auth.Application.Features.Users.CreateUser;

public record CreateUserAccountCommand(string UserName, string Password, string Role, Guid Id, string Email, string Name, string SurName, DateTime Birthday,
    DateTime CreationDate, int Age, string ProfilePhoto, bool IsPremium) : IRequest<Result<string>>;
