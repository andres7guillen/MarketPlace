using CSharpFunctionalExtensions;
using MediatR;

namespace Auth.Application.Features.Users.GetUserToken;

public record GetUserTokenQuery(string username, string password) : IRequest<Result<string>>;
