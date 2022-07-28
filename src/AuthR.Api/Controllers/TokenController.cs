using AuthR.BusinessLogic.Models.Commands.Token;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly IMediator _mediator;

    public TokenController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Revoke")]
    public async Task<ActionResult> RevokeRefreshToken(
        [FromBody] RevokeRefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}