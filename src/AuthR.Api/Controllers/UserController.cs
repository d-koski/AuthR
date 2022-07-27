using AuthR.BusinessLogic.Models.Commands;
using AuthR.BusinessLogic.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserViewModel>> Register(
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result;
    }

    [HttpPost("Authenticate")]
    public async Task<ActionResult<TokensViewModel>> Authenticate(
        [FromBody] AuthenticateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result;
    }
}