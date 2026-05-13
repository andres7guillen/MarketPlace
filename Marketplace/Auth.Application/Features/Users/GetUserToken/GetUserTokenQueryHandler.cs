using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Application.Features.Users.GetUserToken;

public class GetUserTokenQueryHandler : IRequestHandler<GetUserTokenQuery, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public GetUserTokenQueryHandler(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    public async Task<Result<string>> Handle(GetUserTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(request.username, request.password);
        return user.IsFailure 
            ? Result.Failure<string>(user.Error) 
            : Result.Success(CreateToken(user.Value));
    }

    private string CreateToken(Domain.Entities.User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var privateKey = _configuration.GetValue<string>("Authentication:JWT:Key");
        var symetricSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
        var signingCredentials = new SigningCredentials(symetricSigningKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_configuration.GetValue<int>("Authentication:JWT:ExpirationHours")),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
