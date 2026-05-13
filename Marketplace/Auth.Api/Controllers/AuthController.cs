using Auth.Application.Features.Users.GetUserToken;
using Auth.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) 
    { 
        var result = await _mediator.Send(new GetUserTokenQuery(request.UserName, request.Password));
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

}
